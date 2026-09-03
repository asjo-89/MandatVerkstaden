using Services.Dtos;
namespace Services.Interfaces;

public interface IElectionService
{
    Task<IReadOnlyList<ElectionDto>> GetAllYearsAsync();
    Task<OriginalElectionResultSetDto?> AddOriginalResultSetAsync(OriginalElectionResultSetDto dto);
    Task<OriginalElectionResultSetDto?> GetOriginalElectionResultSetByIdAsync(int id, Guid userId);
}
