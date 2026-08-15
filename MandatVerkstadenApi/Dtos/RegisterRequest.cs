using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace MandatVerkstadenApi.Dtos;

public record RegisterRequest : IValidatableObject {
    [Required(ErrorMessage = "Förnamn är obligatoriskt.")]
    public string FirstName { get; init; } = null!;

    [Required(ErrorMessage = "Efternamn är obligatoriskt.")]
    public string LastName { get; init; } = null!;

    [Required(ErrorMessage = "Användarnamn är obligatoriskt.")]
    public string UserName { get; init; } = null!;

    [Required(ErrorMessage = "Email är obligatoriskt.")]
    public string Email { get; init; } = null!;

    [Required(ErrorMessage = "Lösenord är obligatoriskt.")]
    public string Password { get; init; } = null!;

    [Required(ErrorMessage = "Du måste bekräfta lösenordet.")]
    public string MatchingPassword { get; init; } = null!;


    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if(!string.IsNullOrWhiteSpace(UserName) && !Regex.IsMatch(UserName, @"^(?=.*\d)(?!.*[åäöÅÄÖ]).{6,}$"))
        {
            yield return new ValidationResult(
                "Användarnamnet måste vara minst 6 tecken långt och innehålla minst en siffra.",
                [nameof(UserName)]
                );
        }

        if (!string.IsNullOrWhiteSpace(Email) && !new EmailAddressAttribute().IsValid(Email))
        {
            yield return new ValidationResult(
                "Ange en giltig e-postadress.",
                [nameof(Email)]
                );
        }

        if (!string.IsNullOrWhiteSpace(Password) && !Regex.IsMatch(Password, @"^(?=.*\d)(?=.*[a-z])(?=.*[A-Z])(?!.*[åäöÅÄÖ]).{8,}$"))
        {
            yield return new ValidationResult(
                "Lösenordet måste vara minst 8 tecken långt, innehålla minst en siffra, en stor och en liten bokstav (a-z).",
                [nameof(Password)]
                );
        }

        if (!string.IsNullOrWhiteSpace(MatchingPassword) && MatchingPassword != Password)
        {
            yield return new ValidationResult(
                "Lösenorden matchar inte.",
                [nameof(MatchingPassword)]
                );
        }
    }
}

