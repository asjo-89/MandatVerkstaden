using Repositories.Entities;

namespace Repositories.Interfaces;

public interface IElectionRepository
{
    Task<IReadOnlyList<Election>> GetAllYearsAsync();
    Task<OriginalElectionResultSet> AddOriginalElectionResultAsync(OriginalElectionResultSet entity);
    Task<OriginalElectionResultSet?> GetOriginalElectionResultSetByIdAsync(int originalElectionResultSetId);
    Task<bool> ValidateElectionConstituencyIdInElectionResult(int constituencyId, int electionId, int municipalityId);
}
