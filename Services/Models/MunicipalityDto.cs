namespace Services.Models;

public record MunicipalityDto
{
    public required int Id { get; init; }
    public required string ElectionAreaName { get; init; }
    //public required int TotalSeatCount { get; init; }

    public IReadOnlyList<ElectionConstituencyDto> ElectionConstituencies { get; init; } = [];
}
