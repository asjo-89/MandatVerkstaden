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
                    }).ToList(),
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

    //public async Task<OriginalElectionResultSet?> GetOriginalElectionResultSetByIdAsync(int originalElectionResultSetId)
    //{
    //    return await _context.OriginalElectionResultSets
    //        .Where(set => set.Id == originalElectionResultSetId)
    //        .Include(set => set.OriginalConstituencyVoteResults)
    //            .ThenInclude(vr => vr.ElectionConstituency)
    //        .Include(set => set.OriginalConstituencyVoteResults)
    //            .ThenInclude(vr => vr.PoliticalParty)
    //        .Include(set => set.OriginalCouncilSeatAllocations)
    //            .ThenInclude(sa => sa.PoliticalParty)
    //        .Include(set => set.Municipality)
    //        .Include(set => set.Election)
    //        .FirstOrDefaultAsync();
    //}

    //public async Task<OriginalElectionResultSet?> GetElectionResultWithBoardSeatAllocationsByIdAsync(int originalElectionResultSetId, Guid userId)
    //{
    //    return await _context.OriginalElectionResultSets
    //        .Where(set => set.Id == originalElectionResultSetId && set.UserId == userId)
    //        .Include(set => set.OriginalConstituencyVoteResults)
    //        .Include(set => set.OriginalCouncilSeatAllocations)
    //            .ThenInclude(x => x.PoliticalParty)
    //        .Include(set => set.OriginalBoardSeatAllocationSets)
    //            .ThenInclude(x => x.OriginalBoardSeatAllocations)
    //            .ThenInclude(y => y.PoliticalParty)
    //        .FirstOrDefaultAsync();
    //}

    //public async Task<IReadOnlyList<OriginalElectionResultSet?>> GetOriginalResultsWithSeatAllocationsByIdAsync(int id, Guid userId)
    //{
    //    return await _context.OriginalElectionResultSets
    //        .Where(set => set.Id == id && set.UserId == userId)
    //        .Include(set => set.OriginalConstituencyVoteResults)
    //        .Include(set => set.OriginalCouncilSeatAllocations)
    //            .ThenInclude(seats => seats.PoliticalParty)
    //        .ToListAsync();
    //}
}
