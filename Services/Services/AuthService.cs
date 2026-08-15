using Repositories.Entities;
using Repositories.Interfaces;
using Services.Interfaces;

namespace Services.Services
{
    public class AuthService(IUserRepository users, IRefreshTokenRepository refreshTokens, ITokenService tokenService) : IAuthService
    {
        private readonly IUserRepository _users = users;
        private readonly IRefreshTokenRepository _refreshTokens = refreshTokens;
        private readonly ITokenService _tokenService = tokenService;


        public async Task<(bool Success, string? Error)> RegisterAsync(string username, string email, string firstName, string lastName, string password)
        {
            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(email) || password.Length < 8)
                return (false, "Ogiltigt användarnamn, email eller lösenord.");

            if (await _users.UserNameExistsAsync(username))
                return (false, "Användarnamnet är upptaget.");

            if(await _users.EmailExistsAsync(email))
                return (false, "Epostadressen är redan registrerad på ett konto.");

            var newUser = new User
            {
                UserName = username,
                Email = email,
                FirstName = firstName ?? "",
                LastName = lastName ?? "",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(password),
                Role = "User"
            };

            await _users.AddUserAsync(newUser);
            return (true, null);
        }
        public async Task<(AuthResult? Result, AuthTokens? Tokens, string? Error)> LoginAsync(string username, string password)
        {
            var user = await _users.GetByUserNameAsync(username);

            if (user is null || !BCrypt.Net.BCrypt.Verify(password, user.PasswordHash))
                return (null, null, "Ogiltigt användarnamn eller lösenord.");

            var tokens = await IssueTokensAsync(user);
            return (new AuthResult(user.UserName, user.Role), tokens, null);
        }

        public async Task LogoutAsync(string refreshTokenValue)
        {
            var tokenHash = _tokenService.Hash(refreshTokenValue);
            var storedHash = await _refreshTokens.GetRefreshTokenByHashAsync(tokenHash);

            if(storedHash is not null)
            {
                storedHash.RevokedAt = DateTime.UtcNow;
                await _refreshTokens.SaveChangesAsync();
            }
        }

        public async Task<(AuthTokens? Tokens, string? Error)> RefreshTokenAsync(string refreshTokenValue)
        {
            var tokenHash = _tokenService.Hash(refreshTokenValue);
            var storedHash = await _refreshTokens.GetRefreshTokenByHashAsync(tokenHash);

            if(storedHash is null || !storedHash.IsActive)
            {
                if(storedHash is not null)
                    await _refreshTokens.RevokeAllRefreshTokensForUserAsync(storedHash.UserId);
                return (null, "Ogiltig eller utgången refresh-token.");
            }

            storedHash.RevokedAt = DateTime.UtcNow;
            var tokens = await IssueTokensAsync(storedHash.User, storedHash);
            return (tokens, null);
        }


        private async Task<AuthTokens> IssueTokensAsync(User user, RefreshToken? replacesToken = null)
        {
            var accessToken = _tokenService.GenerateAccessToken(user);
            var refreshTokenValue = _tokenService.GenerateRefreshTokenValue();
            var refreshTokenHash = _tokenService.Hash(refreshTokenValue);

            if (replacesToken is not null)
                replacesToken.ReplacedByTokenHash = refreshTokenHash;

            await _refreshTokens.AddRefreshTokenAsync(new RefreshToken
            {
                UserId = user.Id,
                TokenHash = refreshTokenHash,
                ExpiresAt = DateTime.UtcNow.AddDays(7)
            });

            if (replacesToken is not null)
                await _refreshTokens.SaveChangesAsync();

            return new AuthTokens(accessToken, refreshTokenValue);
        }
    }
}
