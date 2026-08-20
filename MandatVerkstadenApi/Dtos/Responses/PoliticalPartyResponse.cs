namespace MandatVerkstadenApi.Dtos.Responses;

public record PoliticalPartyResponse
(
    int? Id,
    string Name,
    bool IsLocal,
    bool IsParliamentary,
    Guid? UserId,
    int? MunicipalityId
);
