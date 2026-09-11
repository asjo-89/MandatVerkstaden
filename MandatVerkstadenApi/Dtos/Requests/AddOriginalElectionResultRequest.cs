using Services.Dtos;
using System.ComponentModel.DataAnnotations;

namespace MandatVerkstadenApi.Dtos.Requests;

public record AddOriginalElectionResultRequest
{
    [Required(ErrorMessage = "Du måste ange antal mandat.")]
    public required int TotalCouncilSeatCount { get; init; }

    [Required(ErrorMessage = "Du måste välja en kommun.")]
    public required int MunicipalityId { get; init; }

    [Required(ErrorMessage = "Du måste välja ett valår.")]
    public required int ElectionYearId { get; init; }



    [Required(ErrorMessage = "Du måste ange ett valresultat.")]
    public ICollection<OriginalConstituencyVoteResultDto> VoteResults { get; init; } = new List<OriginalConstituencyVoteResultDto>();
}



