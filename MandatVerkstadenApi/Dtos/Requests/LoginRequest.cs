using System.ComponentModel.DataAnnotations;

namespace MandatVerkstadenApi.Dtos.Requests;

public record LoginRequest {
    [Required(ErrorMessage = "Du måste ange ett användarnamn.")]
    public required string Username { get; init; }

    [Required(ErrorMessage = "Du måste ange ett lösenord.")]
    public required string Password { get; init; }
}