using Services.Models;
using Services.Services;

namespace Services.Interfaces;

public interface IElectionService
{
    Task<IReadOnlyList<ElectionDto>> GetAllYearsAsync();
    Task<IReadOnlyList<OriginalResultsWithSeatAllocations>?> AddOriginalResultSetAsync(OriginalElectionResultSetDto dto);
    Task<IReadOnlyList<OriginalResultsWithSeatAllocations>> GetOriginalResultsWithSeatAllocationsByIdAsync(int id, Guid userId);
}
