namespace Services.Models;

public record ElectionConstituencyDto
{
    public required int Id { get; init; }
    public required string Name { get; init; } 
    public required int FixedSeatCount { get; init; }
}
