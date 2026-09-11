using Services.Dtos;
namespace Services.Interfaces;

public interface IElectionService
{
    Task<OriginalElectionResultSetDto?> AddOriginalResultSetAsync(OriginalElectionResultSetDto dto);
    Task<OriginalBoardSeatAllocationSetDto?> AddOriginalBoardSeatAllocationSetAsync(OriginalBoardSeatAllocationSetDto dto, Guid userId);

    Task<IReadOnlyList<ElectionDto>> GetAllYearsAsync();
    Task<IReadOnlyList<OriginalElectionResultSetDto>> GetAllOriginalElectionResultSetsAsync(Guid userId);
    Task<OriginalElectionResultSetDto?> GetOriginalElectionResultSetByIdAsync(int id, Guid userId);
}
