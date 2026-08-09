using Microsoft.EntityFrameworkCore;
using Repositories.Data;
using Repositories.Entities;
using Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Repositories.Repositories
{
    public class RefreshTokenRepository(AppDbContext context) : IRefreshTokenRepository
    {
        private readonly AppDbContext _context = context;

        #region CREATE
        public async Task AddRefreshTokenAsync(RefreshToken refreshToken)
        {
            _context.RefreshTokens.Add(refreshToken);
            await _context.SaveChangesAsync();
        }
        #endregion

        #region READ
        public Task<RefreshToken?> GetRefreshTokenByHashAsync(string tokenHash) => 
            _context.RefreshTokens
                .Include(rt => rt.User)
                .SingleOrDefaultAsync(rt => rt.TokenHash == tokenHash);
        #endregion

        #region UPDATE
        public Task SaveChangesAsync() => _context.SaveChangesAsync();
        #endregion

        #region DELETE
        public async Task RevokeAllRefreshTokensForUserAsync(Guid userId)
        {
            var tokens = await _context.RefreshTokens
                .Where(rt => rt.UserId == userId && rt.RevokedAt == null)
                .ToListAsync();

            foreach(var token in tokens)
            {
                token.RevokedAt = DateTime.UtcNow;
                await _context.SaveChangesAsync();
            }
        }

        #endregion
    }
}
