using Repositories.Entities;

namespace Repositories.Interfaces
{
    public interface IMunicipalityRepository
    {
        Task AddAsync(Municipality entity);
        Task<bool> AlreadyExistsAsync(string name);
        Task<Municipality?> GetOneAsync(int municipalityId);
    }
}
