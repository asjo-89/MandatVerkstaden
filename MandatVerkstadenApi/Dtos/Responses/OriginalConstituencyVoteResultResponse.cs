namespace MandatVerkstadenApi.Dtos.Responses;

public record OriginalConstituencyVoteResultResponse
    (
        int Id,
        int NumberOfVotes,
        int PoliticalPartyId,
        string PoliticalPartyName,
        string ElectionConstituencyName
    );


    