namespace Repositories.Dtos;

public record OriginalBoardSeatAllocationSetDto
{
    public int Id { get; init; }
    public int MaxSeatCount { get; init; }
    public int OriginalElectionResultSetId { get; init; }
    public IReadOnlyList<OriginalBoardSeatAllocationDto> OriginalBoardSeatAllocationDtos { get; init; } = [];
}
