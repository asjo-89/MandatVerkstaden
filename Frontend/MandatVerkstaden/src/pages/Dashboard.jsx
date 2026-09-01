import ResultTable from "../components/tables/resultTable"


function Dashboard({ allocatedResults }) {



  return (
    <>
        <ResultTable originalAllocationResults={allocatedResults} />
    </>
  )
}

export default Dashboard