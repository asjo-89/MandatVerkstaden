using Repositories.Entities;

namespace Repositories.Interfaces;

public interface IPoliticalPartyRepository
{
    Task<PoliticalParty> AddAsync(PoliticalParty model);
    Task<PoliticalParty?> GetOneByIdAsync(int id);
    Task<IReadOnlyList<PoliticalParty>> GetAllAsync();
    Task<IReadOnlyList<PoliticalParty>> GetAllParliamentaryPartiesAsync();
}
