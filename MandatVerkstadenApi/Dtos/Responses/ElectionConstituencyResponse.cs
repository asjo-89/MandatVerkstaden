namespace MandatVerkstadenApi.Dtos.Responses;

public record ElectionConstituencyResponse
    (
        int Id,
        string Name,
        int FixedSeatCount
    );