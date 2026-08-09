namespace ValKvotenApi.Dtos;

public record RegisterRequest(string Username, string Email, string FirstName, string LastName, string Password);

