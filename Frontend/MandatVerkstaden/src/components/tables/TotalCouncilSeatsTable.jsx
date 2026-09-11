const TotalCouncilSeatsTable = ({ originalAllocationResults }) => {
    const totalCouncilSeatsForParties = getTotalCouncilSeatsForParties(originalAllocationResults.seatAllocations ?? []);

    function getTotalCouncilSeatsForParties(allocations) {
        const totalSeatsMap = new Map();       
        
        for(const allocation of allocations) {
            const existing = totalSeatsMap.get(allocation.politicalPartyId);

            if(existing) {
                totalSeatsMap.set(allocation.politicalPartyId, 
                { 
                    politicalPartyName: allocation.politicalPartyName, 
                    allocatedSeats: existing.allocatedSeats + 1 
                });
            }
            else {
                totalSeatsMap.set(allocation.politicalPartyId, 
                { 
                    politicalPartyName: allocation.politicalPartyName, 
                    allocatedSeats: 1 
                });
            }
        }
        return Array.from(totalSeatsMap, ([politicalPartyId, data]) => (
            { 
                id: politicalPartyId, 
                politicalPartyId,
                politicalPartyName: data.politicalPartyName,
                allocatedSeats: data.allocatedSeats
            }));
    }

  return (
    <div className="table-container">
                <table className="selected-parties-table">
                    <thead>
                        <tr className="manrope-bold">
                            <th className="text-align-left">Parti</th>
                            <th className="text-align-right">Mandat i KF</th>
                        </tr>
                    </thead>
    
                    <tbody>
                        {totalCouncilSeatsForParties?.map(seat => (
                            <tr key={seat.id}>
                                <td className="text-align-left">{seat.politicalPartyName}</td>
                                <td className="text-align-right">{seat.allocatedSeats}</td>
                            </tr>
                        ))}
                    </tbody>
                </table>
            </div>
  )
}

export default TotalCouncilSeatsTable