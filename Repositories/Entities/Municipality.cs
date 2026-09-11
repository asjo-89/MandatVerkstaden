namespace Repositories.Entities;

public class Municipality
{
    public int Id { get; set; }
    public int MunicipalityCode { get; set; }
    public required string ElectionAreaName { get; set; }



    #region Navigation properties
    public ICollection<ElectionConstituency> ElectionConstituencies { get; set; } = new List<ElectionConstituency>();
    public ICollection<PoliticalParty> PoliticalParties { get; set; } = new List<PoliticalParty>();
    public ICollection<OriginalElectionResultSet> OriginalElectionResultSets { get; set; } = new List<OriginalElectionResultSet>();
    #endregion
}