namespace Repositories.Entities;

public class OriginalBoardSeatAllocation
{
    public int Id { get; set; }
    public int SeatAllocationStep { get; set; }
    public decimal ComparisonNumber { get; set; }
    public decimal AllocationDivisor { get; set; }
    public bool WonByLotDrawing { get; set; }
    public int LotDrawingGroupId { get; set; }



    #region Navigation properties
    public int OriginalBoardSeatAllocationSetId { get; set; }
    public OriginalBoardSeatAllocationSet OriginalBoardSeatAllocationSet { get; set; } = null!;

    public int PoliticalPartyId { get; set; }
    public PoliticalParty PoliticalParty { get; set; } = null!;
    #endregion
}