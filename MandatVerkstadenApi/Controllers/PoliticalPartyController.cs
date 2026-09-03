using MandatVerkstadenApi.Dtos.Requests;
using MandatVerkstadenApi.Dtos.Responses;
using Microsoft.AspNetCore.Mvc;
using Services.Interfaces;
using Services.Dtos;
using System.Security.Claims;

namespace MandatVerkstadenApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PoliticalPartyController(IPoliticalPartyService service) : Controller
{
    private readonly IPoliticalPartyService _service = service;


    [HttpPost("add")]
    public async Task<IActionResult> Add([FromBody]AddPoliticalPartyRequest request)
    {
        if(!ModelState.IsValid)
            return ValidationProblem("Det saknas information i formuläret.");

        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (userIdClaim is null || !Guid.TryParse(userIdClaim, out var userId))
            return Unauthorized("Du måste logga in.");

        var dto = AddRequestToAddDto(request, userId);

        var newParty = await _service.AddAsync(dto);

        return newParty is null
            ? Conflict()
            : Created("Partiet har skapats.", AddDtoToResponse(newParty));
    }

    [HttpGet("get-all")]
    public async Task<IActionResult> GetAll()
    {
        var parties = await _service.GetAllAsync();
        var list = parties.Select(DtoToResponse).ToList();

        return list.Any()
            ? Ok(list)
            : NotFound(new { message = "Inga partier hittades." });
    }

    [HttpGet("get-all-parliamentary")]
    public async Task<IActionResult> GetAllParliamentaryParties()
    {
        var parties = await _service.GetAllParliamentaryPartiesAsync();
        var list = parties.Select(DtoToResponse).ToList();

        return list.Any()
            ? Ok(list)
            : NotFound(new { message = "Inga riksdagspartier hittades." });
    }

    [HttpGet("get-one/{id}")]
    public async Task<IActionResult> GetOneById(int id)
    {
        if (id <= 0)
            return ValidationProblem("Felaktigt id.");

        var party = await _service.GetOneByIdAsync(id);

        return party is null
            ? NotFound(new { message = "Inga partier hittades." })
            : Ok(DtoToResponse(party));
    }

    private static PoliticalPartyResponse DtoToResponse(PoliticalPartyDto dto)
    {
        return new PoliticalPartyResponse
        (
            dto.Id,
            dto.Name,
            dto.IsLocal,
            dto.IsParliamentary,
            dto.UserId ?? null,
            dto.MunicipalityId ?? null
        );
    }

    private static AddPoliticalPartyDto AddRequestToAddDto(AddPoliticalPartyRequest request, Guid userId)
    {
        return new AddPoliticalPartyDto
        {            
            Name = request.Name,
            IsLocal = request.IsLocal,
            IsParliamentary = request.IsParliamentary,
            UserId = userId,
            MunicipalityId = request.MunicipalityId
        };
    }

    private static PoliticalPartyResponse AddDtoToResponse(AddPoliticalPartyDto dto)
    {
        return new PoliticalPartyResponse
        (
            dto.Id,
            dto.Name,
            dto.IsLocal,
            dto.IsParliamentary,
            dto.UserId,
            dto.MunicipalityId
        );
    }

}
