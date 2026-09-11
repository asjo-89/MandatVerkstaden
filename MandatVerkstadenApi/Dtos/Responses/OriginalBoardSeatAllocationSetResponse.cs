namespace MandatVerkstadenApi.Dtos.Responses;

public record OriginalBoardSeatAllocationSetResponse
{
    public int Id { get; init; }
    public int MaxSeatCount { get; init; }
    public int OriginalElectionResultSetId { get; init; }
    public IReadOnlyList<OriginalBoardSeatAllocationResponse> BoardSeatAllocations { get; init; } = [];
}

