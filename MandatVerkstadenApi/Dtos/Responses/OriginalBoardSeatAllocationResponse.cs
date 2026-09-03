namespace MandatVerkstadenApi.Dtos.Responses;

public record OriginalBoardSeatAllocationResponse
    (
        int Id,
        int PoliticalPartyId,
        string PoliticalPartyName,
        int SeatAllocationStep,
        decimal ComparisonNumber,
        decimal AllocationDivisor,
        bool WonByLotDrawing,
        int LotDrawingGroupId
    );