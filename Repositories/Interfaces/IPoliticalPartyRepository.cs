using Repositories.Entities;

namespace Repositories.Interfaces;

public interface IPoliticalPartyRepository
{
    Task<PoliticalParty> AddAsync(PoliticalParty party);
    Task<PoliticalParty?> GetOneByIdAsync(int id);
    Task<IReadOnlyList<PoliticalParty>> GetAllAsync();
    Task<IReadOnlyList<PoliticalParty>> GetAllParliamentaryPartiesAsync();
}
