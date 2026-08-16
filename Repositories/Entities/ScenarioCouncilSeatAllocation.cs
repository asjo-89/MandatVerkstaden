namespace Repositories.Entities;

public class ScenarioCouncilSeatAllocation
{
    public int Id { get; set; }
    public int AllocatedSeat { get; set; }
    public int TotalSeatCountForPartyBeforeAllocation { get; set; }
    public decimal ComparisonNumber { get; set; }
    public decimal AllocationDivisor { get; set; }



    #region Navigation properties
    public int ScenarioId { get; set; }
    public Scenario Scenario { get; set; } = null!;

    public int PoliticalPartyId { get; set; }
    public PoliticalParty PoliticalParty { get; set; } = null!;
    #endregion
}