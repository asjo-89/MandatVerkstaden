const VotesForLastMandateTable = ({ originalAllocationResults }) => {
        const votesNeededForLastMandate = getVotesNeededForLastMandate(originalAllocationResults);

  return (
    <>
      {votesNeededForLastMandate.length > 0 && (
            <div className="table-container">     
                <table className="selected-parties-table">
                    <thead>
                        <tr className="manrope-bold">
                            <th className="text-align-left">Parti</th>
                            <th className="text-align-right">Röster</th>
                        </tr>
                    </thead>
                    <tbody>
                        {votesNeededForLastMandate.map((party) => {
                            if (party.votesNeeded === 0) return null;
                            return (
                                <tr key={party.politicalPartyId}>
                                    <td className="text-align-left">{party.politicalPartyName}</td>
                                    <td className="text-align-right">{party.votesNeeded}</td>
                                </tr>
                            );
                        })}
                    </tbody>
                </table>
            </div>
        )}
    </>
  )
}


function getVotesNeededForLastMandate(allocationResults) {
    const allocations = allocationResults?.seatAllocations ?? [];
    const votes = allocationResults?.voteResults ?? [];
    const lastMandate = [...allocations]
        .reverse()
        .find((allocation) => allocation.allocatedSeats === allocationResults.totalCouncilSeatCount);

    if (!lastMandate || !Number.isFinite(lastMandate.comparisonNumber)) {
        return [];
    }

    const latestAllocationByParty = new Map();
    for (const allocation of allocations) {
        latestAllocationByParty.set(allocation.politicalPartyId, allocation);
    }

    return votes.map((vote) => {
        const isLastMandateWinner = vote.politicalPartyId === lastMandate.politicalPartyId;
        const latestAllocation = latestAllocationByParty.get(vote.politicalPartyId);
        const nextDivisor = latestAllocation
            ? Number(latestAllocation.allocationDivisor) + 2
            : 1.2;
        const currentVotes = Number(vote.numberOfVotes) || 0;
        const votesNeeded = isLastMandateWinner
            ? 0
            : Math.max(0, Math.floor(lastMandate.comparisonNumber * nextDivisor - currentVotes) + 1);

        return {
            politicalPartyId: vote.politicalPartyId,
            politicalPartyName: vote.politicalPartyName,
            votesNeeded
        };
    }).sort((firstParty, secondParty) => firstParty.votesNeeded - secondParty.votesNeeded);
}

export default VotesForLastMandateTable