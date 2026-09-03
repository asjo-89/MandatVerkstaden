namespace MandatVerkstadenApi.Dtos.Responses;

public record OriginalCouncilSeatAllocationResponse
(
    int Id,
    int AllocatedSeats,
    int TotalCouncilSeatCountForParty,
    decimal ComparisonNumber,
    decimal AllocationDivisor,
    int PoliticalPartyId,
    string PoliticalPartyName,
    bool WonByLotDrawing
);
