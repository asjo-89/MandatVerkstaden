using Azure.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.IdentityModel.Tokens;
using Services.Interfaces;
using System.Security.Principal;
using ValKvotenApi.Dtos;

namespace ValKvotenApi.Controllers
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
            var (success, error) = await _authService.RegisterAsync(
                request.Username, 
                request.Email, 
                request.FirstName, 
                request.LastName, 
                request.Password);

            return success ? Ok() : BadRequest(error);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginRequest request)
        {
            var (result, token, error) = await _authService.LoginAsync(
                request.Username, 
                request.Password);

            if (error is not null || token is null || result is null)
                return Unauthorized(error);

            SetAuthCookies(token);
            return Ok(new UserResponse(result.Username, result.Role));
        }

        [HttpPost("refresh")]
        public async Task<IActionResult> RefreshToken()
        {
            if (!Request.Cookies.TryGetValue("request_token", out var refreshTokenValue))
                return Unauthorized();

            var (token, error) = await _authService.RefreshTokenAsync(refreshTokenValue);

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
