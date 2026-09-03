using Microsoft.EntityFrameworkCore;
using Repositories.Entities;
using Repositories.Interfaces;
using Services.Exceptions;
using Services.Interfaces;
using Services.Dtos;

namespace Services.Services;

public class PoliticalPartyService(IPoliticalPartyRepository repo, IUnitOfWork context) : IPoliticalPartyService
{
    private readonly IPoliticalPartyRepository _repo = repo;
    private readonly IUnitOfWork _context = context;

    public async Task<AddPoliticalPartyDto?> AddAsync(AddPoliticalPartyDto party)
    {
        if (party is null)
            throw new ArgumentNullException("Input parameter is null.", nameof(party));

        var newEntity = AddDtoToEntity(party);

        try
        {
            var entity = await _repo.AddAsync(newEntity);

            if (entity is null)
                return null;

            await _context.SaveChangesAsync();
            return EntityToAddDto(entity);
        }
        catch (DbUpdateConcurrencyException ex)
        {
            throw new BusinessRulesException("The entity was updated or deleted by another process.", ex);
        }
        catch (DbUpdateException ex)
        {
            throw new BusinessRulesException("Something went wrong adding new entity to the database.", ex);
        }
    }

    public async Task<IReadOnlyList<PoliticalPartyDto>> GetAllAsync()
    {
        var parties = await _repo.GetAllAsync();
        return parties.Select(EntityToDto).ToList();
    }

    public async Task<PoliticalPartyDto?> GetOneByIdAsync(int id)
    {
        if (id <= 0)
            throw new ArgumentException("Id must be a positive integer.", nameof(id));

        var party = await _repo.GetOneByIdAsync(id);

        return party is null
            ? null
            : EntityToDto(party);
    }

    public async Task<IReadOnlyList<PoliticalPartyDto>> GetAllParliamentaryPartiesAsync()
    {
        var parties = await _repo.GetAllParliamentaryPartiesAsync();
        return parties.Select(EntityToDto).ToList();
    }


    private static PoliticalParty DtoToEntity(PoliticalPartyDto dto)
    {
        return new PoliticalParty
        {
            Id = dto.Id,
            Name = dto.Name,
            IsLocal = dto.IsLocal,
            IsParliamentary = dto.IsParliamentary,
            UserId = dto.UserId ?? null,
            MunicipalityId = dto.MunicipalityId ?? null
        };
    }

    private static PoliticalPartyDto EntityToDto(PoliticalParty entity)
    {
        return new PoliticalPartyDto
        {
            Id = entity.Id,
            Name = entity.Name,
            IsLocal = entity.IsLocal,
            IsParliamentary = entity.IsParliamentary,
            UserId = entity.UserId,
            MunicipalityId = entity.MunicipalityId
        };
    }

    private static PoliticalParty AddDtoToEntity(AddPoliticalPartyDto dto)
    {
        return new PoliticalParty
        {
            Name = dto.Name,
            IsLocal = dto.IsLocal,
            IsParliamentary = dto.IsParliamentary,
            UserId = dto.UserId,
            MunicipalityId = dto.MunicipalityId
        };
    }

    private static AddPoliticalPartyDto EntityToAddDto(PoliticalParty entity)
    {
        return new AddPoliticalPartyDto
        {
            Id = entity.Id,
            Name = entity.Name,
            IsLocal = entity.IsLocal,
            IsParliamentary = entity.IsParliamentary,
            UserId = entity.UserId,
            MunicipalityId = entity.MunicipalityId
        };
    }
}