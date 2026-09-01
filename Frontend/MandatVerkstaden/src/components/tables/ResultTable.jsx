import React from "react"

const ResultTable = ({ originalAllocationResults }) => {
    return (
    <>
        <div className="table-container">
            <table className="selected-parties-table">
                <thead>
                    <tr className="manrope-bold">
                        <th>Mandat nr</th>
                        <th>Parti</th>
                        <th>Totala partimandat</th>
                        <th>Uddatal</th>
                        <th>Jämförelsetal</th>
                        <th>Lottning</th>
                    </tr>
                </thead>

                <tbody>
                    {originalAllocationResults?.map(seat => (
                        <React.Fragment key={seat.seatAllocationId}>
                            <tr key={seat.seatAllocationId}>
                                <td>{seat.allocatedSeat}</td>
                                <td>{seat.politicalPartyName}</td>
                                <td>{seat.totalCouncilSeatCountForParty}</td>
                                <td>{seat.allocationDivisor}</td>
                                <td>{seat.comparisonNumber}</td>
                                <td className="manrope-semibold" style={{ color: seat.wonByLotDrawing ? "green" : "red" }}>{seat.wonByLotDrawing ? "Ja" : "Nej"}</td>
                            </tr>
                            {seat.wonByLotDrawing && seat.seatAllocationId === originalAllocationResults[originalAllocationResults.length - 1] && (
                                <tr key={seat.seatAllocationId}>
                                    <td>{seat.allocatedSeat}</td>
                                    <td>{seat.politicalPartyName}</td>
                                    <td>{seat.totalCouncilSeatCountForParty}</td>
                                    <td>{seat.allocationDivisor}</td>
                                    <td>{seat.comparisonNumber}</td>
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

export default ResultTable