using Repositories.Entities;

namespace Repositories.Interfaces;

public interface IElectionRepository
{
    Task<IReadOnlyList<Election>> GetAllYearsAsync();
    Task<OriginalElectionResultSet> AddOriginalElectionResultAsync(OriginalElectionResultSet entity);
    Task<bool> OriginalElectionResultExistsAsync(Guid userId, int municipalityId, int electionId);
    Task<OriginalElectionResultSet?> GetOriginalElectionResultSetByIdAsync(int originalElectionResultSetId);
    Task<bool> ValidateElectionConstituencyIdInElectionResult(int constituencyId, int electionId, int municipalityId);
    Task<IReadOnlyList<OriginalElectionResultSet?>> GetOriginalResultsWithSeatAllocationsByIdAsync(int id, Guid userId);
}
