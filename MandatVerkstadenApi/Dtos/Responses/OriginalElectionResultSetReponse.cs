namespace MandatVerkstadenApi.Dtos.Responses;

public record OriginalElectionResultSetReponse
{
    public int Id { get; init; }
    public DateTime CreatedDate { get; init; }
    public int TotalCouncilSeatCount { get; init; }
    public MunicipalityResponse Municipality { get; init; } = null!;
    public ElectionResponse Election { get; init; } = null!;

    public OriginalBoardSeatAllocationSetResponse? BoardSeatAllocationSet { get; init; } = null;
    public IReadOnlyList<OriginalConstituencyVoteResultResponse> VoteResults { get; init; } = [];
    public IReadOnlyList<OriginalCouncilSeatAllocationResponse> SeatAllocations { get; init; } = [];
}
