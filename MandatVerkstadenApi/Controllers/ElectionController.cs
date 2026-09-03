using MandatVerkstadenApi.Dtos.Requests;
using MandatVerkstadenApi.Dtos.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.Dtos;
using Services.Interfaces;
using Services.Models;
using System.Security.Claims;

namespace MandatVerkstadenApi.Controllers;


[ApiController]
[Route("api/[controller]")]
public class ElectionController(IElectionService electionService, IPoliticalPartyService politicalPartyService) : Controller
{
    private readonly IElectionService _electionService = electionService;
    private readonly IPoliticalPartyService _politicalPartyService = politicalPartyService;

    [HttpPost("add-election-result")]
    [Authorize]
    public async Task<IActionResult> AddOriginalElectionResult(AddOriginalElectionResultRequest request)
    {
        if (!ModelState.IsValid)
            return ValidationProblem("Du måste fylla i alla fält.");

        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (userIdClaim is null || !Guid.TryParse(userIdClaim, out var userId))
            return Unauthorized("Du måste logga in.");

        var electionResult = AddNewResultRequestToDto(request, userId);

        var addedResult = await _electionService.AddOriginalResultSetAsync(electionResult);

        if (addedResult is null)
            return BadRequest("Något gick fel. Försök igen senare.");

        return Ok(ResultSetDtoToReponse(addedResult));
    }


    [HttpGet("get-all")]
    public async Task<IActionResult> GetAllYears()
    {
        var list = await _electionService.GetAllYearsAsync();
        return list.Any()
            ? Ok(list.Select(e => new ElectionResponse(Id: e.Id, ElectionYear: e.ElectionYear)).ToList())
            : NotFound(new { message = "Inga valår hittades." });
    }

    [HttpGet("get-election-result")]
    public async Task<IActionResult> GetElectionResultSetById(int originalElectionResultSetId)
    {
        if (originalElectionResultSetId <= 0)
            return ValidationProblem("Något gick fel vid hämtning. Försök igen senare.");

        var userClaimId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (userClaimId is null || !Guid.TryParse(userClaimId, out var userId))
            return Unauthorized("Du måste logga in.");

        var dtoResultSet = await _electionService.GetOriginalElectionResultSetByIdAsync(originalElectionResultSetId, userId);

        if (dtoResultSet is null)
            return NotFound("Inga valresultat hittades.");

        return Ok(ResultSetDtoToReponse(dtoResultSet));
    }





    private static OriginalElectionResultSetReponse ResultSetDtoToReponse(OriginalElectionResultSetDto dto)
    {
        return new OriginalElectionResultSetReponse
        {
            Id = dto.Id ?? 0,
            Municipality = new MunicipalityResponse
            (
                dto.MunicipalityId,
                dto.MunicipalityName ?? "",
                new List<ElectionConstituencyResponse>()
            ),
            Election = new ElectionResponse
            (
                dto.ElectionYearId,
                dto.ElectionYear
            ),
            TotalCouncilSeatCount = dto.TotalCouncilSeatCount,
            BoardSeatAllocationSet = dto.BoardSeatAllocationSet == null
            ? null
            : new OriginalBoardSeatAllocationSetResponse
            {
                Id = dto.BoardSeatAllocationSet.Id ?? 0,
                MaxSeatCount = dto.BoardSeatAllocationSet.MaxSeatCount,
                BoardSeatAllocations = dto.BoardSeatAllocationSet.OriginalBoardSeatAllocations
                    .Select(boardSet => new OriginalBoardSeatAllocationResponse
                    (
                        boardSet.Id ?? 0,
                        boardSet.PoliticalPartyId,
                        boardSet.PoliticalPartyName ?? "",
                        boardSet.SeatAllocationStep,
                        boardSet.ComparisonNumber,
                        boardSet.AllocationDivisor,
                        boardSet.WonByLotDrawing,
                        boardSet.LotDrawingGroupId
                    )).ToList()
            },
            SeatAllocations = dto.CouncilSeatAllocations
                .Select(seat => new OriginalCouncilSeatAllocationResponse
                (
                    seat.Id ?? 0,
                    seat.AllocatedSeat,
                    seat.TotalCouncilSeatCountForParty,
                    seat.ComparisonNumber,
                    seat.AllocationDivisor,
                    seat.PoliticalPartyId,
                    seat.PoliticalPartyName ?? "",
                    seat.WonByLotDrawing
                )).ToList(),
            VoteResults = dto.VoteResults
                .Select(vote => new OriginalConstituencyVoteResultResponse
                (
                    vote.Id ?? 0,
                    vote.NumberOfVotes,
                    vote.PoliticalPartyId,
                    vote.PoliticalPartyName ?? "",
                    vote.ElectionConstituencyId,
                    vote.ElectionConstituencyName ?? ""
                )).ToList()
        };
    }

    private static OriginalElectionResultSetDto AddNewResultRequestToDto(AddOriginalElectionResultRequest request, Guid userId)
    {
        return new OriginalElectionResultSetDto
        {
            TotalCouncilSeatCount = request.TotalCouncilSeatCount,
            MunicipalityId = request.MunicipalityId,
            ElectionYearId = request.ElectionYearId,
            UserId = userId,
            VoteResults = request.VoteResults.Select(vr =>
                new OriginalConstituencyVoteResultDto
                {
                    NumberOfVotes = vr.NumberOfVotes,
                    ElectionConstituencyId = vr.ElectionConstituencyId,
                    PoliticalPartyId = vr.PoliticalPartyId
                }
            ).ToList()
        };
    }

    //private static OriginalElectionVoteResultResponse DtoToResponse(OriginalElectionResultSetDto dto)
    //{
    //    return new OriginalElectionVoteResultResponse
    //    (
    //        Id: dto.Id ?? 0,
    //        Municipality: new MunicipalityResponse
    //        (
    //            Id: dto.Municipality?.Id ?? 0,
    //            ElectionAreaName: dto.Municipality?.ElectionAreaName ?? "",
    //            ElectionConstituencies: []
    //        ),
    //        Election: new ElectionResponse
    //        (
    //            Id: dto.Election?.Id ?? 0,
    //            ElectionYear: dto.Election?.ElectionYear ?? 0
    //        ),
    //        PoliticalParties: (dto.PoliticalParties ?? [])
    //            .Select(party => new PoliticalPartyResponse(party.Id, party.Name, null, null, null, null))
    //            .ToList(),
    //        VoteResults: (dto.VoteResults ?? [])
    //            .Select(result =>
    //                new OriginalConstituencyVoteResultResponse
    //                (
    //                    Id: result.Id ?? 0,
    //                    NumberOfVotes: result.NumberOfVotes,
    //                    PoliticalPartyId: result.PoliticalPartyId,
    //                    PoliticalPartyName: result.PoliticalPartyName ?? "",
    //                    ElectionConstituencyName: result.ElectionConstituencyName ?? ""
    //                )
    //            ).ToList(),
    //        SeatAllocations: (dto.SeatAllocations ?? [])
    //            .Select(sa =>
    //                new OriginalCouncilSeatAllocationResponse
    //                (
    //                    Id: sa.Id ?? 0,
    //                    AllocatedSeats: sa.AllocatedSeat,
    //                    TotalSeatCountForPartyBeforeAllocation: sa.TotalCouncilSeatCountForParty,
    //                    ComparisonNumber: sa.ComparisonNumber,
    //                    AllocationDivisor: sa.AllocationDivisor,
    //                    PoliticalPartyName: sa.PoliticalPartyName ?? ""

    //                )
    //            ).ToList(),
    //        ResultsWithSeats: []
    //    );
    //}
}
