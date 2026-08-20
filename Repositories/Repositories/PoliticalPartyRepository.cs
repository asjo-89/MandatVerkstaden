using Microsoft.EntityFrameworkCore;
using Repositories.Data;
using Repositories.Entities;
using Repositories.Interfaces;

namespace Repositories.Repositories;

public class PoliticalPartyRepository(AppDbContext context) : IPoliticalPartyRepository
{
    private readonly AppDbContext _context = context;

    public async Task<PoliticalParty> AddAsync(PoliticalParty party)
    {
        _context.Add(party);
        await _context.SaveChangesAsync();
        return party;
    }

    public async Task<IReadOnlyList<PoliticalParty>> GetAllAsync()
    {
        return await _context.PoliticalParties.ToListAsync();
    }

    public async Task<IReadOnlyList<PoliticalParty>> GetAllParliamentaryPartiesAsync()
    {
        return await _context.PoliticalParties.Where(pp => pp.IsParliamentary == true).ToListAsync();
    }

    public async Task<PoliticalParty?> GetOneByIdAsync(int id)
    {
        return await _context.PoliticalParties.FindAsync(id);
    }
}
