using System.ComponentModel.DataAnnotations;

namespace Services.Dtos;

public record AddOriginalBoardSeatAllocationSetRequest
{
    public int OriginalElectionResultSetId { get; init; }

    [Required(ErrorMessage = "Du måste ange det högsta antalet platser du vill räkna på.")]
    public required int MaxSeatCount { get; init; }
};