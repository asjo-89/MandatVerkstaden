namespace Services.Models;

public record PoliticalPartyDto
{
    public required int Id { get; init; }
    public required string Name { get; init; }
    public required bool IsLocal { get; init; } = false;
    public required bool IsParliamentary { get; init; } = false;

    public Guid? UserId { get; init; }
    public int? MunicipalityId { get; init; }
}
