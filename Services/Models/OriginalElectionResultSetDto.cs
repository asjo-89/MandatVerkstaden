using Repositories.Entities;

namespace Services.Models;

public record OriginalElectionResultSetDto
{
    public int? Id { get; init; }
    public required int TotalCouncilSeatCount { get; init; }
    public int MunicipalityId { get; init; }
    public int ElectionYearId { get; init; }
    public Guid UserId { get; init; }

    public MunicipalityDto? Municipality { get; init; }
    public ElectionDto? Election { get; init; }
    public IReadOnlyList<OriginalConstituencyVoteResultDto> VoteResults { get; init; } = [];
    public IReadOnlyList<OriginalCouncilSeatAllocationDto> SeatAllocations { get; init; } = [];
    public IReadOnlyList<PoliticalPartyDto>? PoliticalParties { get; init; } = [];
}
