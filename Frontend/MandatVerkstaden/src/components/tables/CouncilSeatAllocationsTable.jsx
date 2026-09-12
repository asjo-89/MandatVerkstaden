import React from "react"

const CouncilSeatAllocationsTable = ({ originalAllocationResults }) => {

    return (
    <>
        <div className="table-container">
            <table className="selected-parties-table">
                <thead>
                    <tr className="manrope-bold">
                        <th className="text-align-left">Mandat</th>
                        <th className="text-align-left">Parti</th>
                        {/* <th className="text-align-right">Totala partimandat</th>
                        <th className="text-align-right">Uddatal</th> */}
                        <th className="text-align-right">Jämförelsetal</th>
                        <th className="text-align-right">Lottning</th>
                    </tr>
                </thead>

                <tbody>
                    {originalAllocationResults?.seatAllocations?.map(seat => (
                        <React.Fragment key={seat.id}>
                            <tr key={seat.id}>
                                <td className="text-align-left">{seat.allocatedSeats}</td>
                                <td className="text-align-left">{seat.politicalPartyName}</td>
                                {/* <td>{seat.totalCouncilSeatCountForParty}</td>
                                <td>{seat.allocationDivisor}</td> */}
                                <td className="text-align-right">{seat.comparisonNumber}</td>
                                <td className="manrope-semibold" style={{ color: seat.wonByLotDrawing ? "green" : "red" }}>{seat.wonByLotDrawing ? "Ja" : "Nej"}</td>
                            </tr>
                            {seat.wonByLotDrawing && seat.id === originalAllocationResults.totalCouncilSeatCount && (
                                <tr key={seat.id}>
                                    <td className="text-align-left">{seat.allocatedSeats}</td>
                                    <td className="text-align-left">{seat.politicalPartyName}</td>
                                    {/* <td>{seat.totalCouncilSeatCountForParty}</td>
                                    <td>{seat.allocationDivisor}</td> */}
                                    <td className="text-align-right">{seat.comparisonNumber}</td>
                                    <td className="manrope-semibold" style={{ color: seat.wonByLotDrawing ? "green" : "red" }}>{seat.wonByLotDrawing ? "Ja" : "Nej"}</td>
                                </tr>
                            )}
                        </React.Fragment>
                    ))}
                </tbody>
            </table>
        </div>
        
    </>
  )
}


export default CouncilSeatAllocationsTable