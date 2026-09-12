const ElectionResultCard = ({ originalElectionResult, setAllocatedResults, setShowBoardSeatAllocations, setBoardSeatAllocationSet }) => {
    const handleClick = () => {
        setAllocatedResults(originalElectionResult);
        setBoardSeatAllocationSet(null);

        if(originalElectionResult.boardSeatAllocationSet) {
            setShowBoardSeatAllocations(true);
            console.log("Board seat allocations found:", originalElectionResult.boardSeatAllocationSet);
            setBoardSeatAllocationSet(originalElectionResult.boardSeatAllocationSet);
        }
    };
  return (
    <>
        <div className="election-card" onClick={handleClick}>
            <div className="card-header">
                <h3>{originalElectionResult?.municipality?.electionAreaName} kommun</h3>
                <p>{originalElectionResult?.election?.electionYear}</p>
            </div>
            <div className="card-footer">
                <p>Skapad: {originalElectionResult?.createdDate?.replace('T', ' ').slice(0, 16)}</p>            
            </div>
        </div>
    </>
  )
}

export default ElectionResultCard