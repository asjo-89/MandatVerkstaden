using Repositories.Entities;
using Services.Dtos;
using System.Runtime.CompilerServices;

namespace Services.Helpers;

internal static class BoardSeatAllocationCalculator
{
    private record ComparisonItem(
        int PoliticalPartyId, 
        decimal ComparisonNumber,
        int OriginalElectionResultSetId);


    internal static List<OriginalBoardSeatAllocationDto> CalculateBoardSeatAllocations
        (
            IEnumerable<OriginalCouncilSeatAllocationDto> councilAllocations,
            int maxBoardSeatCount
        )
    {
        if (councilAllocations is null || maxBoardSeatCount <= 0) return new List<OriginalBoardSeatAllocationDto>();

        // List of every political party's total number of allocated seats in the municipality council.
        var totalSeatsForPartyInCouncil = councilAllocations
            .GroupBy(x => x.PoliticalPartyId)
            .Select(g => g.MaxBy(x => x.TotalCouncilSeatCountForParty)!)
            .Select(x => new
            {
                x.PoliticalPartyId,
                x.OriginalElectionResultSetId,
                x.TotalCouncilSeatCountForParty
            })
            .ToList();

        if (totalSeatsForPartyInCouncil.Count <= 0) return new List<OriginalBoardSeatAllocationDto>();

        var partyDivisor = new Dictionary<int, int>();
        var partyBoardAllocations = new List<OriginalBoardSeatAllocationDto>();

        foreach(var party in totalSeatsForPartyInCouncil)
        {
            partyDivisor[party.PoliticalPartyId] = 1;
        }

        // List used to compare all parties comparison number for every seat.
        var comparisonList = new List<ComparisonItem>();

        for (int seat = 1; seat <= maxBoardSeatCount; seat++)
        {
            comparisonList.Clear();

            foreach (var party in totalSeatsForPartyInCouncil)
            {
                int currentDivisor = partyDivisor[party.PoliticalPartyId];
                decimal comparisonNumber = (decimal)party.TotalCouncilSeatCountForParty / currentDivisor;

                comparisonList.Add(new ComparisonItem(party.PoliticalPartyId, comparisonNumber, party.OriginalElectionResultSetId));
            }

            var maxComparisonNumber = comparisonList.Max(x => x.ComparisonNumber);
            var topParties = comparisonList.Where(x => x.ComparisonNumber == maxComparisonNumber).ToList();

            // If multiple parties have the same comparison number a random party is chosen to be allocated the seat.
            ComparisonItem winner = topParties.Count > 1
                ? topParties[Random.Shared.Next(topParties.Count)]
                : topParties[0];

            foreach(var party in topParties)
            {
                bool isWinner = winner.PoliticalPartyId == party.PoliticalPartyId;

                partyBoardAllocations.Add(new OriginalBoardSeatAllocationDto
                {
                    PoliticalPartyId = party.PoliticalPartyId,
                    AllocationDivisor = partyDivisor[party.PoliticalPartyId],
                    ComparisonNumber = party.ComparisonNumber,
                    SeatAllocationStep = seat,
                    WonSeat = isWinner,
                    WonByLotDrawing = isWinner && topParties.Count > 1,
                    LotDrawingGroupId = topParties.Count > 1 ? seat : 0,
                    OriginalBoardSeatAllocationSetId = party.OriginalElectionResultSetId
                });
                
                if(winner.PoliticalPartyId == party.PoliticalPartyId)
                {
                    partyDivisor[winner.PoliticalPartyId] += 1;
                }
            }
        }
        return partyBoardAllocations;
    }

}
