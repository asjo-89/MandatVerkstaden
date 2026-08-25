namespace MandatVerkstadenApi.Dtos.Responses;

public record OriginalElectionVoteResultResponse
    (
        int Id,
        MunicipalityResponse Municipality,
        ElectionResponse Election,
        IReadOnlyList<PoliticalPartyResponse> PoliticalParties,
        IReadOnlyList<OriginalConstituencyVoteResultResponse> VoteResults,
        IReadOnlyList<OriginalCouncilSeatAllocationResponse> SeatAllocations
    );