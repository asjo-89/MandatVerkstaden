using Microsoft.EntityFrameworkCore;
using Repositories.Data;
using Repositories.Entities;
using Repositories.Interfaces;

namespace Repositories.Repositories;

public class MunicipalityRepository(AppDbContext context) : IMunicipalityRepository
{
    private readonly AppDbContext _context = context;

    #region CREATE

    #endregion

    #region READ
    public async Task<Municipality?> GetOneByIdAsync(int id)
    {
        return await _context.Municipalities.FirstOrDefaultAsync(m => m.Id == id);
    }

    public async Task<IReadOnlyList<Municipality>> GetAllAsync()
    {
        return await _context.Municipalities.ToListAsync();
    }

    public async Task<IReadOnlyList<Municipality>> GetAllWithOneConstituencyAsync(int electionYearId)
    {
        return await _context.Municipalities
            .Include(m => m.ElectionConstituencies.Where(e => e.ElectionId == electionYearId))
            .Where(m => m.ElectionConstituencies.Count(e => e.ElectionId == electionYearId) == 1)
            .ToListAsync();
    }

    public async Task<IReadOnlyList<Municipality>> GetAllIncludeConstituenciesAsync()
    {
        return await _context.Municipalities.Include(m => m.ElectionConstituencies).ToListAsync();
    }
    #endregion

    #region UPDATE

    #endregion

    #region DELETE

    #endregion
}
