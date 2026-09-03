namespace Repositories.Entities;

public class PoliticalParty
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public bool IsLocal { get; set; } = false;
    public bool IsParliamentary { get; set; } = false;




    #region Navigation properties
    public Guid? UserId { get; set; }
    public User? User { get; set; }

    public int? MunicipalityId { get; set; }
    public Municipality? Municipality { get; set; }

    public ICollection<OriginalBoardSeatAllocation> OriginalBoardSeatAllocations { get; set; } = [];
    public ICollection<OriginalCouncilSeatAllocation> OriginalCouncilSeatAllocations { get; set; } = [];
    public ICollection<OriginalConstituencyVoteResult> OriginalConstituencyVoteResults { get; set; } = [];
    public ICollection<ScenarioConstituencyVoteResult> ScenarioConstituencyVoteResults { get; set; } = [];
    public ICollection<ScenarioCouncilSeatAllocation> ScenarioCouncilSeatAllocations { get; set; } = [];
    public ICollection<PartyGroup> PartyGroups { get; set; } = [];
    #endregion
}
