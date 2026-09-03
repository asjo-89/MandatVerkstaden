namespace MandatVerkstadenApi.Dtos.Responses;

public record OriginalElectionVoteResultResponse
    (
        int Id,
        MunicipalityResponse Municipality,
        ElectionResponse Election,
        IReadOnlyList<OriginalVoteResultsWithSeatAllocations> ResultsWithSeats,
        IReadOnlyList<PoliticalPartyResponse> PoliticalParties,
        IReadOnlyList<OriginalConstituencyVoteResultResponse> VoteResults,
        IReadOnlyList<OriginalCouncilSeatAllocationResponse> SeatAllocations
    );

public record OriginalVoteResultsWithSeatAllocations
    (
        int Id,
        int PoliticalPartyId,
        string PoliticalPartyName,
        int AllocatedSeat,
        decimal ComparisonNumber,
        int TotalCouncilSeatCountForParty
    );