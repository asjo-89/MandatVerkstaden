using Services.Dtos;
using Services.Models;

namespace Services.Helpers;

internal static class CouncilSeatAllocationCalculator
{
    private record ComparisonItem(
        int PoliticalPartyId, 
        decimal ComparisonNumber, 
        int OriginalElectionResultSetId);

    private const decimal FirstDivisor = 1.2m;
    private const decimal DivisorStep = 2m;
    private const decimal SecondDivisor = 3m;

    private static decimal NextDivisor(decimal current)
    {
        return current == FirstDivisor ? SecondDivisor : current + DivisorStep;
    }


    internal static List<OriginalCouncilSeatAllocationDto> CalculateCouncilSeatAllocations(
        IEnumerable<Dtos.OriginalConstituencyVoteResultDto> votes,
        int totalCouncilSeatCount,
        decimal electoralThreshold)
    {
        if (votes is null || totalCouncilSeatCount == 0) return [];
        var totalNumberOfVotes = votes.Sum(v => v.NumberOfVotes);

        if (totalNumberOfVotes == 0) return [];

        var partiesAboveThreshold = votes
            .Where(v => (decimal)v.NumberOfVotes / totalNumberOfVotes * 100 >= electoralThreshold)
            .ToList();

        if (partiesAboveThreshold.Count == 0) return [];

        var partyAllocations = new List<OriginalCouncilSeatAllocationDto>();
        var partyDivisors = new Dictionary<int, decimal>();
        var totalSeatsAfterAllocation = new Dictionary<int, int>();

        foreach(var partyVote in partiesAboveThreshold)
        {
            partyDivisors.Add(partyVote.PoliticalPartyId, FirstDivisor);
            totalSeatsAfterAllocation.Add(partyVote.PoliticalPartyId, 0);
        }

        var comparisonList = new List<ComparisonItem>();
        for(int i = 1; i <= totalCouncilSeatCount; i++)
        {
            comparisonList.Clear();
            foreach(var partyVote in partiesAboveThreshold)
            {
                var currentDivisor = partyDivisors[partyVote.PoliticalPartyId];
                decimal comparisonNumber = partyVote.NumberOfVotes / currentDivisor;
                
                comparisonList.Add(new ComparisonItem(
                    partyVote.PoliticalPartyId, 
                    comparisonNumber, 
                    partyVote.OriginalElectionResultSetId));
            }

            var maxComparisonNumber = comparisonList.Max(x => x.ComparisonNumber);
            var topParties = comparisonList.Where(x => x.ComparisonNumber == maxComparisonNumber).ToList();

            if (topParties.Count == 0) continue;

            bool wonByLot = false;

            if (i == totalCouncilSeatCount)
            {
                ComparisonItem winner = topParties.Count > 1
                    ? topParties[Random.Shared.Next(topParties.Count)]
                    : topParties[0];

                foreach (var party in topParties)
                {
                    bool isWinner = party.PoliticalPartyId == winner.PoliticalPartyId;

                    if (isWinner)
                        totalSeatsAfterAllocation[party.PoliticalPartyId] += 1;

                    partyAllocations.Add(new OriginalCouncilSeatAllocationDto
                    {
                        PoliticalPartyId = party.PoliticalPartyId,
                        AllocatedSeat = i,
                        ComparisonNumber = party.ComparisonNumber,
                        AllocationDivisor = partyDivisors[party.PoliticalPartyId],
                        TotalCouncilSeatCountForParty = totalSeatsAfterAllocation[party.PoliticalPartyId],
                        OriginalElectionResultSetId = party.OriginalElectionResultSetId,
                        WonByLotDrawing = isWinner && topParties.Count > 1
                    });
                }
                continue;
            }

            ComparisonItem topParty;

            if(topParties.Count > 1)
            {
                topParty = topParties[Random.Shared.Next(topParties.Count)];
                wonByLot = true;
            }
            else
            {
                topParty = topParties[0];
            }

            totalSeatsAfterAllocation[topParty.PoliticalPartyId] += 1;

            partyAllocations.Add(new OriginalCouncilSeatAllocationDto
            {
                PoliticalPartyId = topParty.PoliticalPartyId,
                AllocatedSeat = i,
                ComparisonNumber = topParty.ComparisonNumber,
                AllocationDivisor = partyDivisors[topParty.PoliticalPartyId],
                TotalCouncilSeatCountForParty = totalSeatsAfterAllocation[topParty.PoliticalPartyId],
                OriginalElectionResultSetId = topParty.OriginalElectionResultSetId,
                WonByLotDrawing = wonByLot
            });
            partyDivisors[topParty.PoliticalPartyId] =
                NextDivisor(partyDivisors[topParty.PoliticalPartyId]);

        }
        return partyAllocations;
    }
}
