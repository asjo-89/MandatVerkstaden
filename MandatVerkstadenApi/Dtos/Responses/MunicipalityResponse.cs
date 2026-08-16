namespace MandatVerkstadenApi.Dtos.Responses;

public record MunicipalityResponse
    (
        int Id, 
        string ElectionAreaName, 
        int TotalSeatCount,
        IReadOnlyList<ElectionConstituencyResponse> ElectionConstituencies
    );

