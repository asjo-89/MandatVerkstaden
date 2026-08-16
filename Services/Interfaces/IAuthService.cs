using Services.Models.Auth;

namespace Services.Interfaces;

public interface IAuthService
{
    Task<(bool Success, string? Error)> RegisterAsync(string username, string email, string firstName, string lastName, string password);
    Task<(AuthResult? Result, AuthTokens? Tokens, string? Error)> LoginAsync(string username, string password);
    Task<(AuthTokens? Tokens, string? Error)> RefreshTokenAsync(string refreshTokenValue);
    Task LogoutAsync(string refreshTokenValue);
}
