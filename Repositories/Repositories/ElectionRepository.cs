using Microsoft.EntityFrameworkCore;
using Repositories.Data;
using Repositories.Entities;
using Repositories.Interfaces;

namespace Repositories.Repositories;

public class ElectionRepository(AppDbContext context) : IElectionRepository
{
    private readonly AppDbContext _context = context;

    public Task<OriginalElectionResultSet> AddOriginalElectionResultAsync(OriginalElectionResultSet entity)
    {
        _context.Add(entity);
        return Task.FromResult(entity);
    }

    public async Task<bool> ValidateElectionConstituencyIdInElectionResult(int constituencyId, int electionId, int municipalityId)
    {
        return await _context.ElectionConstituencies
            .AnyAsync(ec => ec.Id == constituencyId && ec.ElectionId == electionId && ec.MunicipalityId == municipalityId);
    }

    public async Task<IReadOnlyList<Election>> GetAllYearsAsync()
    {
        return await _context.Elections.OrderByDescending(e => e.ElectionYear).ToListAsync();
    }


    public async Task<OriginalElectionResultSet?> GetOriginalElectionResultSetByIdAsync(int originalElectionResultSetId)
    {
        return await _context.OriginalElectionResultSets
            .Where(set => set.Id == originalElectionResultSetId)
            .Include(set => set.OriginalConstituencyVoteResults)
                .ThenInclude(vr => vr.ElectionConstituency)
            .Include(set => set.OriginalConstituencyVoteResults)
                .ThenInclude(vr => vr.PoliticalParty)
            .Include(set => set.OriginalCouncilSeatAllocations)
                .ThenInclude(sa => sa.PoliticalParty)
            .Include(set => set.Municipality)
            .Include(set => set.Election)
            .FirstOrDefaultAsync();
    }
}
