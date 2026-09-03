import React from "react"

const ResultTable = ({ originalAllocationResults }) => {
console.log(originalAllocationResults)
const totalVotesCount = originalAllocationResults.voteResults.reduce((sum, vote) => sum + vote.numberOfVotes, 0)


    return (
    <>
        <div className="table-container">
            <table className="selected-parties-table">
                <thead>
                    <tr className="manrope-bold">
                        <th className="table-cell-align-left">Mandat nr</th>
                        <th className="table-cell-align-left">Parti</th>
                        <th className="table-cell-align-right">Totala partimandat</th>
                        <th className="table-cell-align-right">Uddatal</th>
                        <th className="table-cell-align-right">Jämförelsetal</th>
                        <th className="table-cell-align-right">Lottning</th>
                    </tr>
                </thead>

                <tbody>
                    {originalAllocationResults?.seatAllocations?.map(seat => (
                        <React.Fragment key={seat.id}>
                            <tr key={seat.id}>
                                <td className="text-align-left">{seat.allocatedSeats}</td>
                                <td className="text-align-left">{seat.politicalPartyName}</td>
                                <td>{seat.totalCouncilSeatCountForParty}</td>
                                <td>{seat.allocationDivisor}</td>
                                <td className="text-align-right">{seat.comparisonNumber}</td>
                                <td className="manrope-semibold" style={{ color: seat.wonByLotDrawing ? "green" : "red" }}>{seat.wonByLotDrawing ? "Ja" : "Nej"}</td>
                            </tr>
                            {seat.wonByLotDrawing && seat.id === originalAllocationResults.totalCouncilSeatCount && (
                                <tr key={seat.id}>
                                    <td className="text-align-left">{seat.allocatedSeats}</td>
                                    <td className="text-align-left">{seat.politicalPartyName}</td>
                                    <td>{seat.totalCouncilSeatCountForParty}</td>
                                    <td>{seat.allocationDivisor}</td>
                                    <td className="text-align-right">{seat.comparisonNumber}</td>
                                    <td className="manrope-semibold" style={{ color: seat.wonByLotDrawing ? "green" : "red" }}>{seat.wonByLotDrawing ? "Ja" : "Nej"}</td>
                                </tr>
                            )}
                        </React.Fragment>
                    ))}
                </tbody>
            </table>
        </div>
        <div className="table-container">
            <table className="selected-parties-table">
                <thead>
                    <tr>
                        <th className="text-align-left">Parti</th>
                        <th className="text-align-right">Röster</th>
                        <th className="text-align-right">Procent</th>
                    </tr>
                </thead>

                <tbody>
                    {originalAllocationResults.voteResults.map(vote => (
                        <tr key={vote.politicalPartyId}>
                            <td className="text-align-left">{vote.politicalPartyName}</td>
                            <td className="text-align-right">
                                <p>{vote.numberOfVotes ?? ""}</p>
                            </td>
                            <td className="text-align-right">
                                <p>{((vote.numberOfVotes ?? 0) / totalVotesCount * 100).toFixed(2)}%</p>
                            </td>  
                        </tr>
                    ))}
                </tbody>
            </table>
            <p className="total-votes-text">Totalt antal röster: 
                <span className="manrope-bold">{totalVotesCount}</span>
            </p>
        </div>
    </>
  )
}

export default ResultTable