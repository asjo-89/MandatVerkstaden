namespace Repositories.Dtos;

public record OriginalElectionResultSetDto
{
    public int Id { get; init; }
    public int TotalCouncilSeatCount { get; init; }
    public int ElectionId { get; init; }
    public int ElectionYear { get; init; }
    public int MunicipalityId { get; init; }
    public string MunicipalityName { get; init; } = string.Empty;
    public IReadOnlyList<OriginalConstituencyVoteResultDto> OriginalConstituencyVoteResultDtos { get; init; } = [];
    public IReadOnlyList<OriginalCouncilSeatAllocationDto> OriginalCouncilSeatAllocationDtos { get; init; } = [];
    public OriginalBoardSeatAllocationSetDto? OriginalBoardSeatAllocationSetDto { get; init; } = null;
}
