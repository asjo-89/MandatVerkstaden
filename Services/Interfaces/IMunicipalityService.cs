using Services.Models;

namespace Services.Interfaces;

public interface IMunicipalityService
{
    Task<MunicipalityDto?> GetOneByIdAsync(int id);
    Task<IReadOnlyList<MunicipalityDto>> GetAllAsync();
    Task<IReadOnlyList<MunicipalityDto>> GetAllIncludeConstituencyAsync();
}
