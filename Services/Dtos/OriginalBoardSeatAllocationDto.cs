namespace Services.Dtos;

public record OriginalBoardSeatAllocationDto
{
    public int? Id { get; set; }
    public int OriginalBoardSeatAllocationSetId { get; init; }
    public int PoliticalPartyId { get; init; }
    public string PoliticalPartyName { get; init; } = string.Empty;
    public int SeatAllocationStep { get; set; }
    public decimal ComparisonNumber { get; set; }
    public decimal AllocationDivisor { get; set; }
    public bool WonSeat { get; set; }
    public bool WonByLotDrawing { get; set; }
    public int LotDrawingGroupId { get; set; }
}