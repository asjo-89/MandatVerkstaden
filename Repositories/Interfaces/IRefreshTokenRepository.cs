using Repositories.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Repositories.Interfaces
{
    public interface IRefreshTokenRepository
    {
        Task AddRefreshTokenAsync(RefreshToken refreshToken);
        Task<RefreshToken?> GetRefreshTokenByHashAsync(string hash);
        Task RevokeAllRefreshTokensForUserAsync(Guid userId);
        Task SaveChangesAsync();
    }
}
