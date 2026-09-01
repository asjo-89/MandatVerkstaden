using Repositories.Entities;
using Repositories.Interfaces;
using Services.Helpers;
using Services.Interfaces;
using Services.Models;

namespace Services.Services;

public class ElectionService(IElectionRepository repo, IUnitOfWork context) : IElectionService
{
    private readonly IElectionRepository _repo = repo;
    private readonly IUnitOfWork _context = context;

    public async Task<IReadOnlyList<OriginalResultsWithSeatAllocations>?> AddOriginalResultSetAsync(OriginalElectionResultSetDto dto)
    {
        if (dto is null)
            throw new ArgumentNullException(nameof(dto), "The input parameter is null");
               
        if(!dto.VoteResults.Any())
            throw new ArgumentException("VoteResults cannot be an empty list.", nameof(dto.VoteResults));

        var alreadyExists = await _repo.OriginalElectionResultExistsAsync(dto.UserId, dto.MunicipalityId, dto.ElectionYearId);

        if (alreadyExists)
            throw new InvalidOperationException("An OriginalElectionResultSet already exists with the same user id, municipality id and election year id.");

        var constituencyIds = dto.VoteResults.Select(vr => vr.ElectionConstituencyId).Distinct().ToList();

        foreach(var id in constituencyIds)
        {
            var isValid = await _repo.ValidateElectionConstituencyIdInElectionResult(id, dto.ElectionYearId, dto.MunicipalityId);

            if (!isValid)
                throw new ArgumentException($"ElectionConstituencyId {id} does not fit with the ElectionId and MunicipalityId", nameof(dto));
        }
        dto.SeatAllocations = CouncilSeatAllocationCalculator.CalculateCouncilSeatAllocations(dto.VoteResults, dto.TotalCouncilSeatCount, 2);

        var entity = await _repo.AddOriginalElectionResultAsync(DtoToEntity(dto));

        if (entity is null)
            return null;

        await _context.SaveChangesAsync();

        var results = await _repo.GetOriginalResultsWithSeatAllocationsByIdAsync(entity.Id, entity.UserId);

        var allocationsList = results
            .Where(result => result is not null)
            .SelectMany(result => result!.OriginalCouncilSeatAllocations)
            .Select(x => new OriginalResultsWithSeatAllocations
                (
                    SeatAllocationId: x.Id,
                    OriginalSetId: x.OriginalElectionResultSetId,
                    PoliticalPartyId: x.PoliticalPartyId,
                    PoliticalPartyName: x.PoliticalParty.Name,
                    NumberOfVotes: x.OriginalElectionResultSet.OriginalConstituencyVoteResults
                        .Where(a => a.PoliticalPartyId == x.PoliticalPartyId)
                        .Sum(a => a.NumberOfVotes),
                    AllocatedSeat: x.AllocatedSeat,
                    ComparisonNumber: Math.Round(x.ComparisonNumber, 4),
                    AllocationDivisor: x.AllocationDivisor,
                    TotalCouncilSeatCountForParty: x.TotalCouncilSeatCountForParty,
                    WonByLotDrawing: x.WonByLotDrawing
                )).ToList();

        return allocationsList is null
            ? null
            : allocationsList;
    }

    public async Task<IReadOnlyList<ElectionDto>> GetAllYearsAsync()
    {
        var years = await _repo.GetAllYearsAsync();
        return years.Select(e => new ElectionDto(Id: e.Id, ElectionYear: e.ElectionYear)).ToList();
    }

    public async Task<IReadOnlyList<OriginalResultsWithSeatAllocations>> GetOriginalResultsWithSeatAllocationsByIdAsync(int id, Guid userId)
    {
        if (id <= 0)
            throw new ArgumentException(nameof(id), "Input parameter id is invalid.");

        var results = await _repo.GetOriginalResultsWithSeatAllocationsByIdAsync(id, userId);

        var dtoList = results
            .Where(result => result is not null)
            .SelectMany(result => result!.OriginalCouncilSeatAllocations)
            .Select(x => new OriginalResultsWithSeatAllocations
                (
                    SeatAllocationId: x.Id,
                    OriginalSetId: x.OriginalElectionResultSetId,
                    PoliticalPartyId: x.PoliticalPartyId,
                    PoliticalPartyName: x.PoliticalParty.Name,
                    NumberOfVotes: x.OriginalElectionResultSet.OriginalConstituencyVoteResults
                        .Where(a => a.PoliticalPartyId == x.PoliticalPartyId)
                        .Sum(a => a.NumberOfVotes),
                    AllocatedSeat: x.AllocatedSeat,
                    ComparisonNumber: Math.Round(x.ComparisonNumber, 4),
                    AllocationDivisor: x.AllocationDivisor,
                    TotalCouncilSeatCountForParty: x.TotalCouncilSeatCountForParty,
                    WonByLotDrawing: x.WonByLotDrawing
                )).ToList();
        return dtoList ?? [];
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
                TotalCouncilSeatCountForParty = a.TotalCouncilSeatCountForParty,
                ComparisonNumber = a.ComparisonNumber,
                PoliticalPartyId = a.PoliticalPartyId,
                WonByLotDrawing = a.WonByLotDrawing
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
                PoliticalPartyId = vote.PoliticalPartyId,
                PoliticalPartyName = vote.PoliticalParty?.Name ?? "",
                ElectionConstituencyName = vote.ElectionConstituency?.Name ?? "",
            }).ToList(),
            SeatAllocations = (entity.OriginalCouncilSeatAllocations ?? []).Select(a => new OriginalCouncilSeatAllocationDto
            {
                Id = a.Id,
                AllocatedSeat = a.AllocatedSeat,
                TotalCouncilSeatCountForParty = a.TotalCouncilSeatCountForParty,
                ComparisonNumber = a.ComparisonNumber,
                AllocationDivisor = a.AllocationDivisor,
                PoliticalPartyName = a.PoliticalParty?.Name ?? "",
            }).ToList()
        };
    }
}

