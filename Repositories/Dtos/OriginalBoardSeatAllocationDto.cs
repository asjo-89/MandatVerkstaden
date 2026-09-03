namespace Repositories.Dtos;

public record OriginalBoardSeatAllocationDto
{
    public int Id { get; init; }
    public int SeatAllocationStep { get; init; }
    public decimal ComparisonNumber { get; init; }
    public decimal AllocationDivisor { get; init; }
    public bool WonByLotDrawing { get; init; }
    public int LotDrawingGroupId { get; init; }
    public int PoliticalPartyId { get; init; }
    public string PoliticalPartyName { get; init; } = string.Empty;
}