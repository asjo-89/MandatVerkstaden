namespace Repositories.Entities;

public class OriginalCouncilSeatAllocation
{
    public int Id { get; set; }
    public int AllocatedSeat { get; set; }
    public int TotalSeatCountForPartyBeforeAllocation { get; set; }
    public decimal ComparisonNumber { get; set; }
    public decimal AllocationDivisor { get; set; }
    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

    #region Navigation properties
    public int PoliticalPartyId { get; set; }
    public PoliticalParty PoliticalParty { get; set; } = null!;

    public int OriginalElectionResultSetId { get; set; }
    public OriginalElectionResultSet OriginalElectionResultSet { get; set; } = null!;

    #endregion
}