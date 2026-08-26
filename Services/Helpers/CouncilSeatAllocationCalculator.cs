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
        IEnumerable<OriginalConstituencyVoteResultDto> votes,
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
        var seatsBeforeAllocation = new Dictionary<int, int>();

        foreach(var partyVote in partiesAboveThreshold)
        {
            partyDivisors.Add(partyVote.PoliticalPartyId, FirstDivisor);
            seatsBeforeAllocation.Add(partyVote.PoliticalPartyId, 0);
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

            ComparisonItem topParty;
            bool wonByLot = false;

            if(topParties.Count > 1)
            {
                topParty = topParties[Random.Shared.Next(topParties.Count)];
                wonByLot = true;
            }
            else
            {
                topParty = topParties[0];
            }

            var seatsBefore = seatsBeforeAllocation[topParty.PoliticalPartyId];
            seatsBeforeAllocation[topParty.PoliticalPartyId] += 1;

            partyAllocations.Add(new OriginalCouncilSeatAllocationDto
            {
                PoliticalPartyId = topParty.PoliticalPartyId,
                AllocatedSeat = i,
                ComparisonNumber = topParty.ComparisonNumber,
                AllocationDivisor = partyDivisors[topParty.PoliticalPartyId],
                TotalSeatCountForPartyBeforeAllocation = seatsBefore,
                OriginalElectionResultSetId = topParty.OriginalElectionResultSetId,
                WonByLotDrawing = wonByLot
            });
            partyDivisors[topParty.PoliticalPartyId] =
                NextDivisor(partyDivisors[topParty.PoliticalPartyId]);

        }
        return partyAllocations;
    }
}
