using Repositories.Entities;
using Repositories.Interfaces;
using Services.Dtos;
using Services.Helpers;
using Services.Interfaces;

namespace Services.Services;

public class ElectionService(IElectionRepository repo, IUnitOfWork context) : IElectionService
{
    private readonly IElectionRepository _repo = repo;
    private readonly IUnitOfWork _context = context;

    public async Task<OriginalElectionResultSetDto?> AddOriginalResultSetAsync(OriginalElectionResultSetDto dto)
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
        dto.CouncilSeatAllocations = CouncilSeatAllocationCalculator.CalculateCouncilSeatAllocations(dto.VoteResults, dto.TotalCouncilSeatCount, 2);

        var entity = await _repo.AddOriginalElectionResult(DtoToEntity(dto));

        if (entity is null)
            return null;

        await _context.SaveChangesAsync();

        var result = await _repo.GetOriginalElectionResultSetByIdAsync(entity.Id, entity.UserId);

        if (result is null)
            return null;

        return ResultSetEntityToDto(result);
    }

    public async Task<IReadOnlyList<ElectionDto>> GetAllYearsAsync()
    {
        var years = await _repo.GetAllYearsAsync();
        return years.Select(e => new ElectionDto(Id: e.Id, ElectionYear: e.ElectionYear)).ToList();
    }

    public async Task<OriginalElectionResultSetDto?> GetOriginalElectionResultSetByIdAsync(int id, Guid userId)
    {
        if (id <= 0 || userId == Guid.Empty)
            throw new ArgumentException("Input parameters id and/or userId is invalid.");

        var entity = await _repo.GetOriginalElectionResultSetByIdAsync(id, userId);

        if (entity is null)
            throw new InvalidOperationException("No election result set was found.");

        var electionResultSet = new OriginalElectionResultSetDto
        {
            Id = entity.Id,
            TotalCouncilSeatCount = entity.TotalCouncilSeatCount,
            MunicipalityName = entity.MunicipalityName,
            ElectionYearId = entity.MunicipalityId,
            ElectionYear = entity.ElectionYear,
            VoteResults = entity.OriginalConstituencyVoteResultDtos
                .Select(voteResult => new OriginalConstituencyVoteResultDto
                {
                    Id = voteResult.Id,
                    NumberOfVotes = voteResult.NumberOfVotes,
                    PoliticalPartyId = voteResult.PoliticalPartyId,
                    PoliticalPartyName = voteResult.PoliticalPartyName,
                    ElectionConstituencyId = voteResult.ElectionConstituencyId,
                    ElectionConstituencyName = voteResult.ElectionConstituencyName
                }).ToList(),
            CouncilSeatAllocations = entity.OriginalCouncilSeatAllocationDtos
                .Select(councilSeats => new OriginalCouncilSeatAllocationDto
                {
                    Id = councilSeats.Id,
                    PoliticalPartyId = councilSeats.PoliticalPartyId,
                    PoliticalPartyName = councilSeats.PoliticalPartyName,
                    AllocatedSeat = councilSeats.AllocatedSeat,
                    AllocationDivisor = councilSeats.AllocationDivisor,
                    ComparisonNumber = councilSeats.ComparisonNumber,
                    WonByLotDrawing = councilSeats.WonByLotDrawing,
                    TotalCouncilSeatCountForParty = councilSeats.TotalCouncilSeatCountForParty
                }).ToList(),
            BoardSeatAllocationSet = entity.OriginalBoardSeatAllocationSetDto == null
            ? null
            : new OriginalBoardSeatAllocationSetDto
            {
                Id = entity.OriginalBoardSeatAllocationSetDto.Id,
                MaxSeatCount = entity.OriginalBoardSeatAllocationSetDto.MaxSeatCount,
                OriginalBoardSeatAllocations = entity.OriginalBoardSeatAllocationSetDto.OriginalBoardSeatAllocationDtos
                    .Select(boardSeats => new OriginalBoardSeatAllocationDto
                    {
                        Id = boardSeats.Id,
                        AllocationDivisor = boardSeats.AllocationDivisor,
                        SeatAllocationStep = boardSeats.SeatAllocationStep,
                        ComparisonNumber = boardSeats.ComparisonNumber,
                        WonByLotDrawing = boardSeats.WonByLotDrawing,
                        LotDrawingGroupId = boardSeats.LotDrawingGroupId,
                        PoliticalPartyName = boardSeats.PoliticalPartyName,
                        PoliticalPartyId = boardSeats.PoliticalPartyId
                    }).ToList()
            }
        };
        return electionResultSet;
    }



    private static OriginalElectionResultSetDto ResultSetEntityToDto(Repositories.Dtos.OriginalElectionResultSetDto entityDto)
    {
        return new OriginalElectionResultSetDto
        {
            Id = entityDto.Id,
            TotalCouncilSeatCount = entityDto.TotalCouncilSeatCount,
            MunicipalityName = entityDto.MunicipalityName,
            ElectionYearId = entityDto.MunicipalityId,
            ElectionYear = entityDto.ElectionYear,
            VoteResults = entityDto.OriginalConstituencyVoteResultDtos
                .Select(voteResult => new OriginalConstituencyVoteResultDto
                {
                    Id = voteResult.Id,
                    NumberOfVotes = voteResult.NumberOfVotes,
                    PoliticalPartyId = voteResult.PoliticalPartyId,
                    PoliticalPartyName = voteResult.PoliticalPartyName,
                    ElectionConstituencyId = voteResult.ElectionConstituencyId,
                    ElectionConstituencyName = voteResult.ElectionConstituencyName
                }).ToList(),
            CouncilSeatAllocations = entityDto.OriginalCouncilSeatAllocationDtos
                .Select(councilSeats => new OriginalCouncilSeatAllocationDto
                {
                    Id = councilSeats.Id,
                    PoliticalPartyId = councilSeats.PoliticalPartyId,
                    PoliticalPartyName = councilSeats.PoliticalPartyName,
                    AllocatedSeat = councilSeats.AllocatedSeat,
                    AllocationDivisor = councilSeats.AllocationDivisor,
                    ComparisonNumber = councilSeats.ComparisonNumber,
                    WonByLotDrawing = councilSeats.WonByLotDrawing,
                    TotalCouncilSeatCountForParty = councilSeats.TotalCouncilSeatCountForParty
                }).ToList(),
            BoardSeatAllocationSet = entityDto.OriginalBoardSeatAllocationSetDto == null
            ? null
            : new OriginalBoardSeatAllocationSetDto
            {
                Id = entityDto.OriginalBoardSeatAllocationSetDto.Id,
                MaxSeatCount = entityDto.OriginalBoardSeatAllocationSetDto.MaxSeatCount,
                OriginalBoardSeatAllocations = entityDto.OriginalBoardSeatAllocationSetDto.OriginalBoardSeatAllocationDtos
                    .Select(boardSeats => new OriginalBoardSeatAllocationDto
                    {
                        Id = boardSeats.Id,
                        AllocationDivisor = boardSeats.AllocationDivisor,
                        SeatAllocationStep = boardSeats.SeatAllocationStep,
                        ComparisonNumber = boardSeats.ComparisonNumber,
                        WonByLotDrawing = boardSeats.WonByLotDrawing,
                        LotDrawingGroupId = boardSeats.LotDrawingGroupId,
                        PoliticalPartyName = boardSeats.PoliticalPartyName,
                        PoliticalPartyId = boardSeats.PoliticalPartyId
                    }).ToList()
            }
        };
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
            OriginalCouncilSeatAllocations = dto.CouncilSeatAllocations.Select(a => new OriginalCouncilSeatAllocation
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

    //private static Dtos.OriginalElectionResultSetDto EntityToDto(OriginalElectionResultSet entity)
    //{
    //    return new Dtos.OriginalElectionResultSetDto
    //    {
    //        Id = entity.Id,
    //        TotalCouncilSeatCount = entity.TotalCouncilSeatCount,
    //        M = entity.Municipality is null 
    //            ? null 
    //            : new MunicipalityDto
    //        {
    //            Id = entity.Municipality.Id,
    //            ElectionAreaName = entity.Municipality.ElectionAreaName
    //        },
    //        Election = entity.Election is null 
    //            ? null 
    //            : new ElectionDto(Id: entity.Election.Id, ElectionYear: entity.Election.ElectionYear),
    //        VoteResults = (entity.OriginalConstituencyVoteResults ?? []).Select(vote => new OriginalConstituencyVoteResultDto
    //        {
    //            Id = vote.Id,
    //            NumberOfVotes = vote.NumberOfVotes,
    //            PoliticalPartyId = vote.PoliticalPartyId,
    //            PoliticalPartyName = vote.PoliticalParty?.Name ?? "",
    //            ElectionConstituencyName = vote.ElectionConstituency?.Name ?? "",
    //        }).ToList(),
    //        SeatAllocations = (entity.OriginalCouncilSeatAllocations ?? []).Select(a => new OriginalCouncilSeatAllocationDto
    //        {
    //            Id = a.Id,
    //            AllocatedSeat = a.AllocatedSeat,
    //            TotalCouncilSeatCountForParty = a.TotalCouncilSeatCountForParty,
    //            ComparisonNumber = a.ComparisonNumber,
    //            AllocationDivisor = a.AllocationDivisor,
    //            PoliticalPartyName = a.PoliticalParty?.Name ?? "",
    //        }).ToList()
    //    };
    //}
}

