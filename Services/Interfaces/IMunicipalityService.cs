using Services.Dtos;

namespace Services.Interfaces;

public interface IMunicipalityService
{
    Task<MunicipalityDto?> GetOneByIdAsync(int id);
    Task<IReadOnlyList<MunicipalityDto>> GetAllAsync();
    Task<IReadOnlyList<MunicipalityDto>> GetAllIncludeConstituenciesAsync();
    Task<IReadOnlyList<MunicipalityDto>> GetAllWithOneConstituencyAsync(int electionYearId);
}
