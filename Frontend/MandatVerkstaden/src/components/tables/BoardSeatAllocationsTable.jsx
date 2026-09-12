const BoardSeatAllocationsTable = ({ boardSeatAllocationSet, maxSeatCount }) => {
    const maxBoardSeats = maxSeatCount ?? (boardSeatAllocationSet.maxBoardSeats ?? 0);

    // Get the odd-numbered board seats
    const oddBoardSeats = getOddBoardSeats(maxBoardSeats);

    // Create a cumulative seat count per step for each party
    // PartyId -> cumulative seat count per step
    const cumulativeSeatsListPerParty = createCumulativeSeatCountPerStep(boardSeatAllocationSet.boardSeatAllocations ?? [], maxBoardSeats);
    
    // Array of unique parties sorted by name
    const parties = Array.from(
        new Map(boardSeatAllocationSet.boardSeatAllocations?.map(allocation => [allocation.politicalPartyId, allocation.politicalPartyName])
        ), ([id, partyName]) => ({ id, partyName })
    ).sort((a, b) => a.partyName.localeCompare(b.partyName, 'sv'));
        
    // Filtered list of allocations that contains all lot drawings
    const lotDrawingInfo = (boardSeatAllocationSet.boardSeatAllocations ?? [])
        .filter(allocation => allocation.lotDrawingGroupId)
        .sort((a, b) => a.seatAllocationStep - b.seatAllocationStep);

    // Map to store information if party won a seat by lot drawing
    // PartyId -> array of lot drawing events
    const lotEventsForParty = new Map();

    for(const row of lotDrawingInfo) {
        if(!lotEventsForParty.has(row.politicalPartyId)) {
            lotEventsForParty.set(row.politicalPartyId, []);
        }

        lotEventsForParty.get(row.politicalPartyId).push({
            step: row.seatAllocationStep,
            won: row.wonByLotDrawing
        })
    }

    // Get the table cell content with information whether the party was involved in a lot drawing and the resulting seat count
    function getTableCellContentWithLotEvents(step, partyId, cumulativeSeatsListPerParty, lotEventsForParty) {
        const currentSeats = cumulativeSeatsListPerParty.get(step)?.get(partyId) ?? 0;
        const partyEvents = lotEventsForParty.get(partyId)?.filter(event => event.step === step);

        if(!partyEvents || partyEvents.length === 0) {
            return {currentSeats, inLotDrawing: false};
        }
        
        const seatDifference = partyEvents.reduce(
            (sum, event) => sum + (event.won ? -1 : 1),
            0
        );
        const alternativeSeats = currentSeats + seatDifference;
        const [lower, higher] = [currentSeats, alternativeSeats].sort((a, b) => a - b);

        return {currentSeats, inLotDrawing: true, lower, higher, won: partyEvents.some(event => event.won)};
    }

    // Function to create a cumulative seat count per step for each party
    function createCumulativeSeatCountPerStep(allocations, maxBoardSeats) {

        // Filter the allocations to only include those that were won
        const wonSeats = allocations
            .filter(allocation => allocation.wonSeat)
            .sort((a, b) => a.seatAllocationStep - b.seatAllocationStep);

        const cumulativeSeatsPerStep = new Map();
        const totalSeatsPerPartySoFar = new Map();

        let seatIndex = 0;

        // Iterate through each allocation step and calculate the cumulative seat count for each party
        for(let step = 1; step <= maxBoardSeats; step++) {
            while(seatIndex < wonSeats.length && wonSeats[seatIndex].seatAllocationStep === step) {
                const allocatedSeat = wonSeats[seatIndex];

                totalSeatsPerPartySoFar
                    .set(allocatedSeat.politicalPartyId, (totalSeatsPerPartySoFar.get(allocatedSeat.politicalPartyId) ?? 0) + 1);

                seatIndex++;
            }
            cumulativeSeatsPerStep.set(step, new Map(totalSeatsPerPartySoFar));
        }
        return cumulativeSeatsPerStep;
    }

    // Function to get odd-numbered board seats
    function getOddBoardSeats(maxBoardSeats) {
        const steps = [];
        if(maxBoardSeats >= 5) {
            for(let seat = 5; seat <= maxBoardSeats; seat += 2) {
                steps.push(seat);
            }
        }
        return steps;
    }

  return (
    <>
        <div className="table-container">
            <table className="selected-parties-table">
                <thead>
                    <tr className="manrope-bold">
                        <th className="text-align-left">Parti</th>
                        {oddBoardSeats.map((seat) => (
                            <th key={seat}>{seat}-nämnd</th>
                        ))}                    
                    </tr>
                </thead>
                <tbody>
                    {parties.map((party) => (
                        <tr key={party.id}>
                            <td className="text-align-left">{party.partyName}</td>
                            {oddBoardSeats.map((seat) => {
                                const cellInfo = getTableCellContentWithLotEvents(seat, party.id, cumulativeSeatsListPerParty, lotEventsForParty);
                                if(!cellInfo.inLotDrawing) {
                                    return <td key={seat}>{cellInfo.currentSeats ?? ''}</td>;
                                }

                                return(
                                    <td key={seat} className={`lot-drawing ${cellInfo.won ? 'lot-drawing-won' : ''}`}>
                                        {cellInfo.lower} eller {cellInfo.higher}
                                    </td>
                                )
                            })}
                        </tr>
                    ))}
                </tbody>
            </table>
        </div>
    </>
  )
}

export default BoardSeatAllocationsTable