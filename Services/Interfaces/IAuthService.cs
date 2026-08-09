using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.Text;

namespace Services.Interfaces;

public record AuthResult(string Username, string Role);
public record AuthTokens(string AccessToken, string RefreshToken);

public interface IAuthService
{
    Task<(bool Success, string? Error)> RegisterAsync(string username, string email, string firstName, string lastName, string password);
    Task<(AuthResult? Result, AuthTokens? Tokens, string? Error)> LoginAsync(string username, string password);
    Task<(AuthTokens? Tokens, string? Error)> RefreshTokenAsync(string refreshTokenValue);
    Task LogoutAsync(string refreshTokenValue);
}
