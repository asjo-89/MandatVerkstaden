using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;

namespace MandatVerkstadenApi.Dtos.Requests;

public record AddPoliticalPartyRequest : IValidatableObject
{
    [Required(ErrorMessage = "Du måste ange ett namn.")]
    public string Name { get; init; } = null!;

    [Required(ErrorMessage = "Ange om det är ett lokalt parti eller ej.")]
    public bool IsLocal { get; init; }
    public bool IsParliamentary { get; init; } = false;
    public int? MunicipalityId { get; init; }


    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if(IsLocal && MunicipalityId is null)
        {
            yield return new ValidationResult
                (
                    "Du måste ange en kommun när partiet är lokalt.",
                    [nameof(MunicipalityId)]
                );
        }
    }
}