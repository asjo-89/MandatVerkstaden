using Repositories.Dtos;
using Repositories.Entities;

namespace Repositories.Interfaces;

public interface IElectionRepository
{
    Task<IReadOnlyList<Election>> GetAllYearsAsync();
    Task<OriginalElectionResultSet> AddOriginalElectionResult(OriginalElectionResultSet entity);
    Task<OriginalBoardSeatAllocationSet> AddOriginalBoardSeatAllocation(OriginalBoardSeatAllocationSet entity);

    Task<bool> OriginalElectionResultExistsAsync(Guid userId, int municipalityId, int electionId);
    Task<bool> OriginalBoardSeatAllocationExistsAsync(int originalElectionResultId, int maxSeatCount);

    Task<OriginalElectionResultSetDto?> GetOriginalElectionResultSetByIdAsync(int originalElectionResultSetId, Guid userId);
    //Task<OriginalBoardSeatAllocationSet?> GetBoardSeatAllocationSetByIdAsync(int originalElectionResultSetId);
    //Task<IReadOnlyList<OriginalElectionResultSet?>> GetOriginalResultsWithouncilSeatAllocationsByIdAsync(int id, Guid userId);

    Task<bool> ValidateElectionConstituencyIdInElectionResult(int constituencyId, int electionId, int municipalityId);
}
