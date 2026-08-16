using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Services.Interfaces;
using MandatVerkstadenApi.Dtos;
using MandatVerkstadenApi.Dtos.Requests;
using Services.Models.Auth;

namespace MandatVerkstadenApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [EnableRateLimiting("AuthPolicy")]
    public class AuthController(IAuthService authService) : ControllerBase
    {
        private readonly IAuthService _authService = authService;

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterRequest request)
        {
            if(!ModelState.IsValid)
            {
                return ValidationProblem(ModelState);
            }

            (bool success, string? error) = await _authService.RegisterAsync(
                request.UserName, 
                request.Email, 
                request.FirstName, 
                request.LastName, 
                request.Password);

            return success ? Ok() : BadRequest(new { message = error });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginRequest request)
        {
            (AuthResult? result, AuthTokens? token, string? error) = await _authService.LoginAsync(
                request.Username, 
                request.Password);

            if (error is not null || token is null || result is null)
                return Unauthorized(new
                {
                    message = error
                });

            SetAuthCookies(token);
            return Ok(new UserResponse(result.Username, result.Role));
        }

        [HttpPost("refresh")]
        public async Task<IActionResult> RefreshToken()
        {
            if (!Request.Cookies.TryGetValue("request_token", out var refreshTokenValue))
                return Unauthorized();

            (AuthTokens? token, string? error) = await _authService.RefreshTokenAsync(refreshTokenValue);

            if(error is not null || token is null)
            {
                ClearAuthCookies();
                return Unauthorized(error);
            }

            SetAuthCookies(token);
            return Ok();
        }

        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            if (Request.Cookies.TryGetValue("refresh_token", out var refreshTokenValue))
                await _authService.LogoutAsync(refreshTokenValue);

            ClearAuthCookies();
            return Ok();
        }

        [HttpGet("current-user")]
        [Authorize]
        public IActionResult CurrentUser()
        {
            return Ok(new UserResponse(
                User.Identity?.Name ?? "",
                User.FindFirst(System.Security.Claims.ClaimTypes.Role)?.Value ?? ""));
        }


        private void SetAuthCookies(AuthTokens tokens)
        {
            SetCookie("access_token", tokens.AccessToken, TimeSpan.FromMinutes(15));
            SetCookie("refresh_token", tokens.RefreshToken, TimeSpan.FromMinutes(15));
        }

        private void ClearAuthCookies()
        {
            Response.Cookies.Delete("access_token");
            Response.Cookies.Delete("refresh_token");
        }

        private void SetCookie(string tokenName, string tokenValue, TimeSpan timeSpan)
        {
            Response.Cookies.Append(tokenName, tokenValue, 
                new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true,
                    SameSite = SameSiteMode.None,
                    Expires = DateTimeOffset.UtcNow.Add(timeSpan)
                });
        }
    }
}
