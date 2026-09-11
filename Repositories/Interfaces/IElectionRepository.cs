using Repositories.Dtos;
using Repositories.Entities;

namespace Repositories.Interfaces;

public interface IElectionRepository
{
    Task<OriginalElectionResultSet> AddOriginalElectionResult(OriginalElectionResultSet entity);
    Task<OriginalBoardSeatAllocationSet> AddOriginalBoardSeatAllocation(OriginalBoardSeatAllocationSet entity);

    Task<bool> OriginalElectionResultExistsAsync(Guid userId, int municipalityId, int electionId);
    Task<bool> OriginalBoardSeatAllocationExistsAsync(int originalElectionResultId, int maxSeatCount);

    Task<IReadOnlyList<Election>> GetAllYearsAsync();
    Task<IReadOnlyList<OriginalElectionResultSetDto>> GetAllOriginalElectionResultSetsAsync(Guid userId);
    Task<OriginalBoardSeatAllocationSetDto?> GetOriginalBoardSeatAllocationSetAsync(int originalElectionResultSetId, int maxSeatCount);
    Task<OriginalElectionResultSetDto?> GetOriginalElectionResultSetByIdAsync(int originalElectionResultSetId, Guid userId);

    Task<bool> ValidateElectionConstituencyIdInElectionResult(int constituencyId, int electionId, int municipalityId);
}
