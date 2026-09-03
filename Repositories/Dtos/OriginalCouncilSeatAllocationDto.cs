namespace Repositories.Dtos;

public record OriginalCouncilSeatAllocationDto
{
    public int Id { get; init; }
    public int AllocatedSeat { get; init; }
    public int TotalCouncilSeatCountForParty { get; init; }
    public decimal ComparisonNumber { get; init; }
    public decimal AllocationDivisor { get; init; }
    public bool WonByLotDrawing { get; init; }
    public int PoliticalPartyId { get; init; }
    public string PoliticalPartyName { get; init; } = string.Empty;
}