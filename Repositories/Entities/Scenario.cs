namespace Repositories.Entities;

public class Scenario
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;



    #region Navigation properties
    public Guid UserId { get; set; }
    public User User { get; set; } = null!;

    public int OriginalElectionResultSetId { get; set; }
    public OriginalElectionResultSet OriginalElectionResultSet { get; set; } = null!;

    public ICollection<PartyGroup> PartyGroups { get; set; } = new List<PartyGroup>();
    public ICollection<ScenarioConstituencyVoteResult> ScenarioConstituencyVoteResults { get; set; } = new List<ScenarioConstituencyVoteResult>();
    public ICollection<ScenarioCouncilSeatAllocation> ScenarioCouncilSeatAllocations { get; set; } = new List<ScenarioCouncilSeatAllocation>();
    #endregion 
}