using Services.Models;

namespace Services.Interfaces;

public interface IPoliticalPartyService
{
    Task<AddPoliticalPartyDto?> AddAsync(AddPoliticalPartyDto party);
    Task<PoliticalPartyDto?> GetOneByIdAsync(int id);
    Task<IReadOnlyList<PoliticalPartyDto>> GetAllAsync();
    Task<IReadOnlyList<PoliticalPartyDto>> GetAllParliamentaryPartiesAsync();
}
