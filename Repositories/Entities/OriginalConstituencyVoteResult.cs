namespace Repositories.Entities;

public class OriginalConstituencyVoteResult
{
    public int Id { get; set; }
    public int NumberOfVotes { get; set; }




    #region Navigation properties
    public int OriginalElectionResultSetId { get; set; }
    public OriginalElectionResultSet OriginalElectionResultSet { get; set; } = null!;

    public int ElectionConstituencyId { get; set; }
    public ElectionConstituency ElectionConstituency { get; set; } = null!;

    public int PoliticalPartyId { get; set; }
    public PoliticalParty PoliticalParty { get; set; } = null!;
    #endregion
}