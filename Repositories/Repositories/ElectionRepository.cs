using Microsoft.EntityFrameworkCore;
using Repositories.Data;
using Repositories.Dtos;
using Repositories.Entities;
using Repositories.Interfaces;
using System.Linq.Expressions;

namespace Repositories.Repositories;

public class ElectionRepository(AppDbContext context) : IElectionRepository
{
    private readonly AppDbContext _context = context;

    public Task<OriginalElectionResultSet> AddOriginalElectionResult(OriginalElectionResultSet entity)
    {
        _context.Add(entity);
        return Task.FromResult(entity);
    }

    public Task<OriginalBoardSeatAllocationSet> AddOriginalBoardSeatAllocation(OriginalBoardSeatAllocationSet entity)
    {
        _context.Add(entity);
        return Task.FromResult(entity);
    }

    public async Task<bool> OriginalElectionResultExistsAsync(Guid userId, int municipalityId, int electionId)
    {
        return await _context.OriginalElectionResultSets
            .AnyAsync(x => x.UserId == userId && x.MunicipalityId == municipalityId && x.ElectionId == electionId);
    }

    public async Task<bool> OriginalBoardSeatAllocationExistsAsync(int originalElectionResultId, int maxSeatCount)
    {
        return await _context.OriginalBoardSeatAllocationSets
            .AnyAsync(x => x.OriginalElectionResultSetId == originalElectionResultId && x.MaxSeatCount == maxSeatCount);
    }

    public async Task<bool> ValidateElectionConstituencyIdInElectionResult(int constituencyId, int electionId, int municipalityId)
    {
        return await _context.ElectionConstituencies
            .AnyAsync(ec => ec.Id == constituencyId && ec.ElectionId == electionId && ec.MunicipalityId == municipalityId);
    }

    public async Task<IReadOnlyList<Election>> GetAllYearsAsync()
    {
        return await _context.Elections.OrderByDescending(e => e.ElectionYear).ToListAsync();
    }

    public async Task<IReadOnlyList<OriginalElectionResultSetDto>> GetAllOriginalElectionResultSetsAsync(Guid userId)
    {
        var electionDtos = await _context.OriginalElectionResultSets
            .Where(x => x.UserId == userId)
            .AsNoTracking()
            .Select(x => new OriginalElectionResultSetDto
            {
                Id = x.Id,
                CreatedDate = x.CreatedDate,
                TotalCouncilSeatCount = x.TotalCouncilSeatCount,
                ElectionId = x.ElectionId,
                ElectionYear = x.Election.ElectionYear,
                MunicipalityId = x.Municipality.Id,
                MunicipalityName = x.Municipality.ElectionAreaName ?? "",
                OriginalCouncilSeatAllocationDtos = x.OriginalCouncilSeatAllocations
                .Select(y => new OriginalCouncilSeatAllocationDto
                {
                    Id = y.Id,
                    AllocatedSeat = y.AllocatedSeat,
                    TotalCouncilSeatCountForParty = y.TotalCouncilSeatCountForParty,
                    ComparisonNumber = y.ComparisonNumber,
                    AllocationDivisor = y.AllocationDivisor,
                    WonByLotDrawing = y.WonByLotDrawing,
                    PoliticalPartyId = y.PoliticalPartyId,
                    PoliticalPartyName = y.PoliticalParty.Name ?? ""
                }).OrderBy(y => y.AllocatedSeat).ToList(),
                OriginalConstituencyVoteResultDtos = x.OriginalConstituencyVoteResults
                .Select(y => new OriginalConstituencyVoteResultDto
                {
                    Id = y.Id,
                    NumberOfVotes = y.NumberOfVotes,
                    PoliticalPartyId = y.PoliticalPartyId,
                    PoliticalPartyName = y.PoliticalParty.Name ?? "",
                    ElectionConstituencyId = y.ElectionConstituencyId,
                    ElectionConstituencyName = y.ElectionConstituency.Name ?? ""
                }).OrderByDescending(y => y.NumberOfVotes).ToList(),
                OriginalBoardSeatAllocationSetDto = x.OriginalBoardSeatAllocationSet == null
                ? null
                : new OriginalBoardSeatAllocationSetDto
                {
                    Id = x.OriginalBoardSeatAllocationSet.Id,
                    MaxSeatCount = x.OriginalBoardSeatAllocationSet.MaxSeatCount,
                    OriginalElectionResultSetId = x.OriginalBoardSeatAllocationSet.OriginalElectionResultSetId,
                    OriginalBoardSeatAllocationDtos = x.OriginalBoardSeatAllocationSet.OriginalBoardSeatAllocations
                        .Select(y => new OriginalBoardSeatAllocationDto
                        {
                            Id = y.Id,
                            SeatAllocationStep = y.SeatAllocationStep,
                            ComparisonNumber = y.ComparisonNumber,
                            AllocationDivisor = y.AllocationDivisor,
                            WonSeat = y.WonSeat,
                            WonByLotDrawing = y.WonByLotDrawing,
                            LotDrawingGroupId = y.LotDrawingGroupId,
                            PoliticalPartyId = y.PoliticalPartyId,
                            PoliticalPartyName = y.PoliticalParty.Name ?? ""
                        }).ToList()
                }
            }).ToListAsync();

        return electionDtos;
    }

    public async Task<OriginalBoardSeatAllocationSetDto?> GetOriginalBoardSeatAllocationSetAsync(int originalElectionResultSetId, int maxSeatCount)
    {
        return await _context.OriginalBoardSeatAllocationSets
            .Where(set => set.OriginalElectionResultSetId == originalElectionResultSetId && set.MaxSeatCount == maxSeatCount)
            .Select(x => new OriginalBoardSeatAllocationSetDto
            {
                Id = x.Id,
                MaxSeatCount = x.MaxSeatCount,
                OriginalElectionResultSetId = x.OriginalElectionResultSetId,
                OriginalBoardSeatAllocationDtos = x.OriginalBoardSeatAllocations
                    .Select(a => new OriginalBoardSeatAllocationDto
                    {
                        Id = a.Id,
                        SeatAllocationStep = a.SeatAllocationStep,
                        ComparisonNumber = a.ComparisonNumber,
                        AllocationDivisor = a.AllocationDivisor,
                        PoliticalPartyId = a.PoliticalPartyId,
                        PoliticalPartyName = a.PoliticalParty.Name ?? "",
                        WonSeat = a.WonSeat,
                        WonByLotDrawing = a.WonByLotDrawing,
                        LotDrawingGroupId = a.LotDrawingGroupId
                    }).ToList()
            }).FirstOrDefaultAsync();
    }

    public async Task<OriginalElectionResultSetDto?> GetOriginalElectionResultSetByIdAsync(int originalElectionResultSetId, Guid userId)
    {
        return await _context.OriginalElectionResultSets
            .AsNoTracking()
            .Where(resultSet => resultSet.Id == originalElectionResultSetId && resultSet.UserId == userId)
            .Select(resultSet => new OriginalElectionResultSetDto
            {
                Id = resultSet.Id,
                ElectionId = resultSet.ElectionId,
                ElectionYear = resultSet.Election.ElectionYear,
                MunicipalityName = resultSet.Municipality.ElectionAreaName,
                MunicipalityId = resultSet.MunicipalityId,
                TotalCouncilSeatCount = resultSet.TotalCouncilSeatCount,
                OriginalConstituencyVoteResultDtos = resultSet.OriginalConstituencyVoteResults
                    .Select(voteResults => new OriginalConstituencyVoteResultDto
                    {
                        Id = voteResults.Id,
                        NumberOfVotes = voteResults.NumberOfVotes,
                        PoliticalPartyId = voteResults.PoliticalPartyId,
                        PoliticalPartyName = voteResults.PoliticalParty.Name,
                        ElectionConstituencyName = voteResults.ElectionConstituency.Name
                    }).OrderBy(x => x.NumberOfVotes).ToList(),
                OriginalCouncilSeatAllocationDtos = resultSet.OriginalCouncilSeatAllocations
                    .Select(councilSeats => new OriginalCouncilSeatAllocationDto
                    {
                        Id = councilSeats.Id,
                        AllocatedSeat = councilSeats.AllocatedSeat,
                        ComparisonNumber = councilSeats.ComparisonNumber,
                        TotalCouncilSeatCountForParty = councilSeats.TotalCouncilSeatCountForParty,
                        AllocationDivisor = councilSeats.AllocationDivisor,
                        WonByLotDrawing = councilSeats.WonByLotDrawing,
                        PoliticalPartyId = councilSeats.PoliticalPartyId,
                        PoliticalPartyName = councilSeats.PoliticalParty.Name
                    }).OrderBy(x => x.AllocatedSeat).ThenByDescending(x => x.WonByLotDrawing).ToList(),
                OriginalBoardSeatAllocationSetDto = resultSet.OriginalBoardSeatAllocationSet == null 
                ? null 
                : new OriginalBoardSeatAllocationSetDto
                {
                    Id = resultSet.OriginalBoardSeatAllocationSet.Id,
                    MaxSeatCount = resultSet.OriginalBoardSeatAllocationSet.MaxSeatCount,
                    OriginalBoardSeatAllocationDtos = resultSet.OriginalBoardSeatAllocationSet.OriginalBoardSeatAllocations
                        .Select(boardSeat => new OriginalBoardSeatAllocationDto
                        {
                            Id = boardSeat.Id,
                            SeatAllocationStep = boardSeat.SeatAllocationStep,
                            ComparisonNumber = boardSeat.ComparisonNumber,
                            AllocationDivisor = boardSeat.AllocationDivisor,
                            WonByLotDrawing = boardSeat.WonByLotDrawing,
                            LotDrawingGroupId = boardSeat.LotDrawingGroupId,
                            PoliticalPartyId = boardSeat.PoliticalPartyId,
                            PoliticalPartyName = boardSeat.PoliticalParty.Name
                        }).ToList()
                }
            }).FirstOrDefaultAsync();
    }

}
