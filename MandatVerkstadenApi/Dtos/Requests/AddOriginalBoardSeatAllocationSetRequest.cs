using System.ComponentModel.DataAnnotations;

namespace MandatVerkstadenApi.Dtos.Requests;

public record AddOriginalBoardSeatAllocationSetRequest
{
    [Required(ErrorMessage = "Koppling måste finnas till ett valresultat.")]
    public int OriginalElectionResultSetId { get; init; }

    [Required(ErrorMessage = "Du måste ange max antal platser för beräkningen.")]
    public int MaxSeatCount { get; init; }
}
