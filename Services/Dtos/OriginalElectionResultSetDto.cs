namespace Services.Dtos;

public record OriginalElectionResultSetDto
{
    public int? Id { get; init; }
    public DateTime CreatedDate { get; init; }
    public required int TotalCouncilSeatCount { get; init; }
    public int MunicipalityId { get; init; }
    public string MunicipalityName { get; init; } = string.Empty;
    public int ElectionYearId { get; init; }
    public int ElectionYear { get; init; }
    public Guid UserId { get; init; }

    //public MunicipalityDto? Municipality { get; init; }
    //public ElectionDto? Election { get; init; }
    public OriginalElectionConstituencyDto ElectionConstituency { get; init; } = null!;
    public IReadOnlyList<OriginalConstituencyVoteResultDto> VoteResults { get; init; } = [];
    public IEnumerable<OriginalCouncilSeatAllocationDto> CouncilSeatAllocations { get; set; } = [];
    public OriginalBoardSeatAllocationSetDto? BoardSeatAllocationSet { get; set; } = null;
    //public IReadOnlyList<PoliticalPartyDto>? PoliticalParties { get; init; } = [];
}
