namespace Services.Dtos;

public record OriginalResultsWithSeatAllocations
    (
        int SeatAllocationId,
        int OriginalSetId,
        int PoliticalPartyId,
        string PoliticalPartyName,
        int NumberOfVotes,
        int AllocatedSeat,
        decimal ComparisonNumber,
        decimal AllocationDivisor,
        int TotalCouncilSeatCountForParty,
        bool WonByLotDrawing
    );
