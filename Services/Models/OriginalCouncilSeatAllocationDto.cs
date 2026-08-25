using Repositories.Entities;

namespace Services.Models;

public record OriginalCouncilSeatAllocationDto
{
    public int? Id { get; set; }
    public int AllocatedSeat { get; set; }
    public int TotalSeatCountForPartyBeforeAllocation { get; set; }
    public decimal ComparisonNumber { get; set; }
    public decimal AllocationDivisor { get; set; }
    public string PoliticalPartyName { get; init; } = string.Empty;
    public int PoliticalPartyId { get; init; }
    public int OriginalElectionResultSetId { get; init; }
}
