using MandatVerkstadenApi.Dtos.Requests;
using MandatVerkstadenApi.Dtos.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> AddOriginalElectionResult(AddOriginalElectionResultRequest request)
    {
        if (!ModelState.IsValid)
            return ValidationProblem("Du måste fylla i alla fält.");

        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (userIdClaim is null || !Guid.TryParse(userIdClaim, out var userId))
            return Unauthorized("Du måste logga in.");

        var electionResult = RequestToDto(request, userId);

        var addedResult = await _electionService.AddOriginalResultSetAsync(electionResult);

        if (addedResult is null)
            return BadRequest("Något gick fel. Försök igen senare.");

        return Ok(DtoToResponse(addedResult));
    }


    [HttpGet("get-all")]
    public async Task<IActionResult> GetAllYears()
    {
        var list = await _electionService.GetAllYearsAsync();
        return list.Any()
            ? Ok(list.Select(e => new ElectionResponse(Id: e.Id, ElectionYear: e.ElectionYear)).ToList())
            : NotFound(new { message = "Inga valår hittades." });
    }




    private static OriginalElectionResultSetDto RequestToDto(AddOriginalElectionResultRequest request, Guid userId)
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

    private static OriginalElectionVoteResultResponse DtoToResponse(OriginalElectionResultSetDto dto)
    {
        return new OriginalElectionVoteResultResponse
        (
            Id: dto.Id ?? 0,
            Municipality: new MunicipalityResponse
            (
                Id: dto.Municipality?.Id ?? 0,
                ElectionAreaName: dto.Municipality?.ElectionAreaName ?? "",
                ElectionConstituencies: []
            ),
            Election: new ElectionResponse
            (
                Id: dto.Election?.Id ?? 0,
                ElectionYear: dto.Election?.ElectionYear ?? 0
            ),
            PoliticalParties: (dto.PoliticalParties ?? [])
                .Select(party => new PoliticalPartyResponse(party.Id, party.Name, null, null, null, null))
                .ToList(),
            VoteResults: (dto.VoteResults ?? [])
                .Select(result =>
                    new OriginalConstituencyVoteResultResponse
                    (
                        Id: result.Id ?? 0,
                        NumberOfVotes: result.NumberOfVotes,
                        PoliticalPartyId: result.PoliticalPartyId,
                        PoliticalPartyName: result.PoliticalPartyName ?? "",
                        ElectionConstituencyName: result.ElectionConstituencyName ?? ""
                    )
                ).ToList(),
            SeatAllocations: (dto.SeatAllocations ?? [])
                .Select(sa =>
                    new OriginalCouncilSeatAllocationResponse
                    (
                        Id: sa.Id ?? 0,
                        AllocatedSeats: sa.AllocatedSeat,
                        TotalSeatCountForPartyBeforeAllocation: sa.TotalSeatCountForPartyBeforeAllocation,
                        ComparisonNumber: sa.ComparisonNumber,
                        AllocationDivisor: sa.AllocationDivisor,
                        PoliticalPartyName: sa.PoliticalPartyName ?? ""

                    )
                ).ToList()
        );
    }
}
