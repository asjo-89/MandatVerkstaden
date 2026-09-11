namespace Repositories.Entities;

public class ElectionConstituency
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public int FixedSeatCount { get; set; }



    #region Navigation properties
    public int ElectionId { get; set; }
    public Election Election { get; set; } = null!;

    public int MunicipalityId { get; set; }
    public Municipality Municipality { get; set; } = null!;

    public ICollection<OriginalConstituencyVoteResult> OriginalConstituencyVoteResults { get; set; } = new List<OriginalConstituencyVoteResult>();
    public ICollection<ScenarioConstituencyVoteResult> ScenarioConstituencyVoteResults { get; set; } = new List<ScenarioConstituencyVoteResult>();
    #endregion
}