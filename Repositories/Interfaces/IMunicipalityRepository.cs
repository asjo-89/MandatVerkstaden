using Repositories.Entities;

namespace Repositories.Interfaces;

public interface IMunicipalityRepository
{
    Task<Municipality?> GetOneByIdAsync(int id);
    Task<IReadOnlyList<Municipality>> GetAllAsync();
    Task<IReadOnlyList<Municipality>> GetAllIncludeConstituencyAsync();
}
