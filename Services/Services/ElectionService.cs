using Microsoft.EntityFrameworkCore;
using Repositories.Data;
using Repositories.Entities;
using Repositories.Interfaces;
using Services.Interfaces;
using Services.Models;

namespace Services.Services;

public class ElectionService(IElectionRepository repo, IUnitOfWork context) : IElectionService
{
    private readonly IElectionRepository _repo = repo;
    private readonly IUnitOfWork _context = context;

    public async Task<OriginalElectionResultSetDto?> AddOriginalResultSetAsync(OriginalElectionResultSetDto dto)
    {
        if (dto is null)
            throw new ArgumentNullException("The input parameter is null", nameof(dto));
               
        var entity = _repo.AddOriginalElectionResultAsync(DtoToEntity(dto));
        await _context.SaveChangesAsync();

        if (entity is null)
            return null!;

        var seatAllocations = await _repo.GetOriginalElectionResultSetByIdAsync(entity.Result.Id);

        return seatAllocations is null
            ? null
            : EntityToDto(seatAllocations);
    }

    public async Task<IReadOnlyList<ElectionDto>> GetAllYearsAsync()
    {
        var years = await _repo.GetAllYearsAsync();
        return years.Select(e => new ElectionDto(Id: e.Id, ElectionYear: e.ElectionYear)).ToList();
    }


    private static OriginalElectionResultSet DtoToEntity(OriginalElectionResultSetDto dto)
    {
        return new OriginalElectionResultSet
        {
            ElectionId = dto.ElectionYearId,
            MunicipalityId = dto.MunicipalityId,
            UserId = dto.UserId,
            TotalCouncilSeatCount = dto.TotalCouncilSeatCount,
            OriginalConstituencyVoteResults = dto.VoteResults.Select(vote => new OriginalConstituencyVoteResult
            {
                NumberOfVotes = vote.NumberOfVotes,
                ElectionConstituencyId = vote.ElectionConstituencyId,
                PoliticalPartyId = vote.PoliticalPartyId
            }).ToList(),
            OriginalCouncilSeatAllocations = dto.SeatAllocations.Select(a => new OriginalCouncilSeatAllocation
            {
                AllocatedSeat = a.AllocatedSeat,
                AllocationDivisor = a.AllocationDivisor,
                TotalSeatCountForPartyBeforeAllocation = a.TotalSeatCountForPartyBeforeAllocation,
                ComparisonNumber = a.ComparisonNumber,
                PoliticalPartyId = a.PoliticalPartyId,
            }).ToList()
        };
    }

    private static OriginalElectionResultSetDto EntityToDto(OriginalElectionResultSet entity)
    {
        return new OriginalElectionResultSetDto
        {
            Id = entity.Id,
            TotalCouncilSeatCount = entity.TotalCouncilSeatCount,
            Municipality = entity.Municipality is null 
                ? null 
                : new MunicipalityDto
            {
                Id = entity.Municipality.Id,
                ElectionAreaName = entity.Municipality.ElectionAreaName
            },
            Election = entity.Election is null 
                ? null 
                : new ElectionDto(Id: entity.Election.Id, ElectionYear: entity.Election.ElectionYear),
            VoteResults = (entity.OriginalConstituencyVoteResults ?? []).Select(vote => new OriginalConstituencyVoteResultDto
            {
                Id = vote.Id,
                NumberOfVotes = vote.NumberOfVotes,
                PoliticalPartyName = vote.PoliticalParty?.Name ?? "",
                ElectionConstituencyName = vote.ElectionConstituency?.Name ?? "",
            }).ToList(),
            SeatAllocations = (entity.OriginalCouncilSeatAllocations ?? []).Select(a => new OriginalCouncilSeatAllocationDto
            {
                Id = a.Id,
                AllocatedSeat = a.AllocatedSeat,
                TotalSeatCountForPartyBeforeAllocation = a.TotalSeatCountForPartyBeforeAllocation,
                ComparisonNumber = a.ComparisonNumber,
                AllocationDivisor = a.AllocationDivisor,
                PoliticalPartyName = a.PoliticalParty?.Name ?? "",
            }).ToList()
        };
    }
}
