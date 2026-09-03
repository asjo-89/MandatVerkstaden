namespace Services.Dtos;

public record OriginalBoardSeatAllocationSetDto
{
    public int? Id { get; init; }
    public int OriginalElectionResultSetId { get; init; }
    public required int MaxSeatCount { get; init; }
    public IReadOnlyList<OriginalBoardSeatAllocationDto> OriginalBoardSeatAllocations { get; set; } = [];
};
