namespace Services.Models;

public record OriginalConstituencyVoteResultDto
{
    public int? Id { get; init; }
    public required int NumberOfVotes { get; init; }
    public string? PoliticalPartyName { get; init; }
    public string? ElectionConstituencyName { get; init; }
    public int PoliticalPartyId { get; init; }
    public int ElectionConstituencyId { get; init; }
    public int OriginalElectionResultSetId { get; init; }
};

