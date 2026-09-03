namespace Repositories.Dtos;

public record OriginalConstituencyVoteResultDto
{
    public int Id { get; init; }
    public int NumberOfVotes { get; init; }
    public int PoliticalPartyId { get; init; }
    public string PoliticalPartyName { get; init; } = string.Empty;
    public int ElectionConstituencyId { get; init; }
    public string ElectionConstituencyName { get; init; } = string.Empty;
}
