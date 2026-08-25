namespace MandatVerkstadenApi.Dtos.Responses;

public record MunicipalityResponse
    (
        int Id, 
        string ElectionAreaName, 
        IReadOnlyList<ElectionConstituencyResponse>? ElectionConstituencies
    );

