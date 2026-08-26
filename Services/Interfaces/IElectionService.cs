using Services.Models;

namespace Services.Interfaces;

public interface IElectionService
{
    Task<IReadOnlyList<ElectionDto>> GetAllYearsAsync();
    Task<OriginalElectionResultSetDto?> AddOriginalResultSetAsync(OriginalElectionResultSetDto dto);
}
