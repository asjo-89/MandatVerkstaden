const VoteResultsTable = ({ originalAllocationResults }) => {
  const totalVotesCount = originalAllocationResults.voteResults.reduce((sum, vote) => sum + vote.numberOfVotes, 0)
  
  return (
    <>
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

export default VoteResultsTable