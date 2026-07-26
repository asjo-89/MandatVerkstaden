using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Repositories.Data;
using Repositories.Entities;
using Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Repositories.Repositories
{
    public class MunicipalityRepository(AppDbContext context) : IMunicipalityRepository
    {
        private readonly AppDbContext _context = context;

        #region CREATE
        public async Task AddAsync(Municipality model)
        {
            _context.Add(model);
            await _context.SaveChangesAsync();
        }
        #endregion

        #region READ
        public async Task<bool> AlreadyExistsAsync(string name)
        {
            return await _context.Municipalities.AnyAsync(m => m.Name == name);
        }

        public async Task<Municipality?> GetOneAsync(int municipalityId)
        {
            var entity = await _context.Municipalities.FindAsync(municipalityId);
            return entity;
        }
        #endregion

        #region UPDATE

        #endregion

        #region DELETE

        #endregion
    }
}
