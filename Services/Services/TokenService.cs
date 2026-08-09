using Microsoft.IdentityModel.Tokens;
using Repositories.Entities;
using Services.Interfaces;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Services.Services
{
    public class TokenService(string key, string issuer, string audience) : ITokenService
    {
        private readonly string _key = key;
        private readonly string _issuer = issuer;
        private readonly string _audience = audience;

        public string GenerateAccessToken(User user)
        {
            if(user is null)
            {
                return null!;
            }

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.Role, user.Role),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var credentials = new SigningCredentials(
                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_key)),
                SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _issuer,
                audience: _audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(15),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public string GenerateRefreshTokenValue()
        {
            var bytes = RandomNumberGenerator.GetBytes(32);
            return Convert.ToBase64String(bytes);
        }

        public string Hash(string value)
        {
            var bytes = SHA256.HashData(Encoding.UTF8.GetBytes(value));
            return Convert.ToHexString(bytes);
        }
    }
}
