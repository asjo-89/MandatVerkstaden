namespace Repositories.Entities;

public class Municipality
{
    public int Id { get; set; }
    public int MunicipalityCode { get; set; }
    public required string ElectionAreaName { get; set; }



    #region Navigation properties
    public ICollection<ElectionConstituency> ElectionConstituencies { get; set; } = [];
    public ICollection<PoliticalParty> PoliticalParties { get; set; } = [];
    public ICollection<OriginalElectionResultSet> OriginalElectionResultSets { get; set; } = [];
    #endregion
}