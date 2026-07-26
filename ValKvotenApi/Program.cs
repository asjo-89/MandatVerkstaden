using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Diagnostics;
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

builder.Host.UseSerilog((context, configuration) =>
{
    configuration
        .WriteTo.Console()
        .WriteTo.File(
            Path.Combine(builder.Environment.ContentRootPath, "Logs", "log.txt"),
            rollingInterval: RollingInterval.Day,
            rollOnFileSizeLimit: true);
});

// Add services to the container.
builder.Services.AddControllers();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(
    builder.Configuration.GetConnectionString("DevConnectionString")));

builder.Services.AddScoped<IMunicipalityService, MunicipalityService>();
builder.Services.AddScoped<IMunicipalityRepository, MunicipalityRepository>();

builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo { Title = "ValKvoten API", Version = "v1" });
});

var jwtIssuer = builder.Configuration["Jwt:Issuer"]
    ?? throw new InvalidOperationException("Jwt:Issuer is missing.");
var jwtAudience = builder.Configuration["Jwt:Audience"]
    ?? throw new InvalidOperationException("Jwt:Audience is missing.");
var jwtKey = builder.Configuration["Jwt:Key"]
    ?? throw new InvalidOperationException("Jwt:Key is missing.");

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(o =>
    {
        o.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtIssuer,
            ValidAudience = jwtAudience,
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(jwtKey)
            )
        };
    });

builder.Services.AddAuthorization();

var app = builder.Build();

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
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();
