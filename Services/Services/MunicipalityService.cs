using Repositories.Entities;
using Repositories.Interfaces;
using Services.Interfaces;
using Services.Dtos;

namespace Services.Services;

public class MunicipalityService(IMunicipalityRepository repo) : IMunicipalityService
{
    private readonly IMunicipalityRepository _repo = repo;

    public async Task<MunicipalityDto?> GetOneByIdAsync(int id)
    {
        if(id <= 0)
            throw new ArgumentException("Id must be a positive integer.", nameof(id));

        var municipality = await _repo.GetOneByIdAsync(id);
        return municipality is null
            ? null
            : EntityToDto(municipality);
    }

    public async Task<IReadOnlyList<MunicipalityDto>> GetAllAsync()
    {
        var municipalities = await _repo.GetAllAsync();

        return municipalities.Select(EntityToDto).ToList();
    }

    public async Task<IReadOnlyList<MunicipalityDto>> GetAllWithOneConstituencyAsync(int electionYearId)
    {
        var municipalities = await _repo.GetAllWithOneConstituencyAsync(electionYearId);

        return municipalities.Select(EntityToDto).ToList();
    }

    public async Task<IReadOnlyList<MunicipalityDto>> GetAllIncludeConstituenciesAsync()
    {
        var municipalities = await _repo.GetAllIncludeConstituenciesAsync();

        return municipalities.Select(EntityToDto).ToList();
    }       


    private static MunicipalityDto EntityToDto(Municipality entity)
    {
        return new MunicipalityDto
        {
            Id = entity.Id,
            ElectionAreaName = entity.ElectionAreaName,
            ElectionConstituencies = entity.ElectionConstituencies?
                .Select(ec => new ElectionConstituencyDto
                (
                    Id: ec.Id,
                    Name: ec.Name,
                    FixedSeatCount: ec.FixedSeatCount
                )).ToList() ?? new List<ElectionConstituencyDto>()

        };
    }
}
