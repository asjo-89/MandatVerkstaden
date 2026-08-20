using Azure;
using MandatVerkstadenApi.Dtos.Requests;
using MandatVerkstadenApi.Dtos.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Services.Interfaces;
using Services.Models;

namespace MandatVerkstadenApi.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class MunicipalityController(IMunicipalityService service) : ControllerBase
{
    private readonly IMunicipalityService _service = service;

    [HttpGet("get-all")]
    public async Task<IActionResult> GetAll()
    {
        var list = await _service.GetAllAsync();
        var response = list.Select(DtoToResponse);
        return response.Any() ? Ok(response) : NotFound(new { message = "Det finns inga kommuner att visa."});
    }

    [HttpGet("get-all-include")]
    public async Task<IActionResult> GetAllIncludeConstituency()
    { 
        var list = await _service.GetAllIncludeConstituencyAsync();
        var response = list.Select(DtoToResponse);
        return response.Any()? Ok(response) : NotFound(new { message = "Det finns inga kommuner att visa."});
    }

    [HttpGet("get-one/{id}")]
    public async Task<IActionResult> GetOneById(int id)
    {
        if(id <= 0)
            return ValidationProblem("Felaktigt id.");

        var municipality = await _service.GetOneByIdAsync(id);

        return municipality is null
            ? NotFound("Kommunen hittades inte.")
            : Ok(DtoToResponse(municipality));
    }


    private static MunicipalityResponse DtoToResponse (MunicipalityDto dto)
    {
        var constituencies = dto.ElectionConstituencies?
            .Select(ec =>
            new ElectionConstituencyResponse
            (
                ec.Id,
                ec.Name,
                ec.FixedSeatCount)
            ).ToList() ?? new List<ElectionConstituencyResponse>();

        return new MunicipalityResponse
            (
                dto.Id,
                dto.ElectionAreaName,
                dto.TotalSeatCount,
                constituencies
            );
        
    }
}
