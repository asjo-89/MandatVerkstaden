namespace MandatVerkstadenApi.Dtos.Responses;

public record OriginalCouncilSeatAllocationResponse
(
    int Id,
    int AllocatedSeats,
    int TotalSeatCountForPartyBeforeAllocation,
    decimal ComparisonNumber,
    decimal AllocationDivisor,
    string PoliticalPartyName
);
