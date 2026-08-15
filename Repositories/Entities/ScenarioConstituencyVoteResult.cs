namespace Repositories.Entities;

public class ScenarioConstituencyVoteResult
{
    public int Id { get; set; }
    public int NumberOfVotes { get; set; }
    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

    #region Navigation properties
    public int PoliticalPartyId { get; set; }
    public PoliticalParty PoliticalParty { get; set; } = null!;

    public int ScenarioId { get; set; }
    public Scenario Scenario { get; set; } = null!;

    public int ElectionConstituencyId { get; set; }
    public ElectionConstituency ElectionConstituency { get; set; } = null!;
    #endregion
}