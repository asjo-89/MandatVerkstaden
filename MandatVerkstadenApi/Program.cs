using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi;
using Repositories.Data;
using Repositories.Interfaces;
using Repositories.Repositories;
using Serilog;
using Services.Exceptions;
using Services.Interfaces;
using Services.Services;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Writes errors to file
builder.Host.UseSerilog((context, configuration) =>
{
    configuration
        .WriteTo.Console()
        .WriteTo.File(
            Path.Combine(builder.Environment.ContentRootPath, "Logs", "log.txt"),
            rollingInterval: RollingInterval.Day,
            rollOnFileSizeLimit: true);
});

var jwtKey = builder.Configuration["Jwt:Key"]
    ?? throw new InvalidOperationException("Jwt:Key is missing.");
var jwtIssuer = builder.Configuration["Jwt:Issuer"] ?? "MyApi";
var jwtAudience = builder.Configuration["Jwt:Audience"] ?? "MyApiClients";
var keyBytes = Encoding.UTF8.GetBytes(jwtKey);

// Add services to the container.

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(
    builder.Configuration.GetConnectionString("DevConnectionString")));

builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo { Title = "MandatVerkstadens API", Version = "v1" });
});

builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IMunicipalityRepository, MunicipalityRepository>();
builder.Services.AddScoped<IMunicipalityService, MunicipalityService>();
builder.Services.AddScoped<IPoliticalPartyRepository, PoliticalPartyRepository>();
builder.Services.AddScoped<IPoliticalPartyService, PoliticalPartyService>();
builder.Services.AddScoped<IElectionRepository, ElectionRepository>();
builder.Services.AddScoped<IElectionService, ElectionService>();
builder.Services.AddScoped<IUnitOfWork>(sp => sp.GetRequiredService<AppDbContext>());

builder.Services.AddSingleton<ITokenService>(new TokenService(jwtKey, jwtIssuer, jwtAudience));

const string CorsPolicy = "Frontend";
builder.Services.AddCors(options =>
{
    options.AddPolicy(CorsPolicy, policy =>
    {
        policy.WithOrigins("http://localhost:5173")
        .AllowCredentials()
        .AllowAnyHeader()
        .AllowAnyMethod();
    });
});

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = jwtIssuer,
            ValidateAudience = true,
            ValidAudience = jwtAudience,
            ValidateLifetime = true,
            ClockSkew = TimeSpan.FromSeconds(30),
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(keyBytes)
        };
        options.Events = new JwtBearerEvents
        {
            OnMessageReceived = context =>
            {
                if (context.Request.Cookies.TryGetValue("access_token", out var token))
                {
                    context.Token = token;
                }
                return Task.CompletedTask;
            }
        };
    });

// All controllers demand authorization
builder.Services.AddAuthorizationBuilder()
    .SetFallbackPolicy(new AuthorizationPolicyBuilder()
        .RequireAuthenticatedUser()
        .Build());

builder.Services.AddControllers();

builder.Services.AddRateLimiter(options =>
{
    options.AddFixedWindowLimiter("AuthPolicy", opt =>
    {
        opt.Window = TimeSpan.FromMinutes(5);
        opt.PermitLimit = 10;
        opt.QueueLimit = 0;
    });
});

var app = builder.Build();

// Global handler for exceptions
app.UseExceptionHandler(errorApplication =>
    {
        errorApplication.Run(async context =>
        {
            var exceptionFeature = context.Features.Get<IExceptionHandlerFeature>();
            var exception = exceptionFeature?.Error ?? new Exception("Okänt fel.");
            var logger = context.RequestServices.GetRequiredService<ILogger<Program>>();

            var (statusCode, message) = exception switch
            {
                NotFoundException => (StatusCodes.Status404NotFound, exception.Message),
                ConflictException => (StatusCodes.Status409Conflict, exception.Message),
                BusinessRulesException => (StatusCodes.Status400BadRequest, exception.Message),
                _ => (StatusCodes.Status500InternalServerError, "Ett oväntat fel inträffade.")
            };

            if(statusCode == StatusCodes.Status500InternalServerError)
            {
                logger.LogError(exception, "Ett oväntat fel inträffade vid {Path}", context.Request.Path);
            }
            else
            {
                logger.LogWarning("Ett fel inträffade med statuskod ({StatusCode}) och Path {Path} med meddelande: {Message}.", statusCode, context.Request.Path, message);
            }

            context.Response.StatusCode = statusCode;
            context.Response.ContentType = "application/json";
            await context.Response.WriteAsJsonAsync(new { error = message });
        });
    }
);

if (app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/error");
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseCors(CorsPolicy);

if(!app.Environment.IsDevelopment())
{
    app.UseRateLimiter();
}

app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();
