// import AddScenarioForm from '../components/forms/AddScenarioForm';
import AddElectionResultForm from '../components/forms/AddElectionResultForm';
import API_URL from '../ApiUrl';
import NormalizeErrors from '../components/helpers/NormalizeErrors';
import { useEffect, useState } from 'react';
import { ApiFetch } from '../components/helpers/ApiFetch';
import CouncilSeatAllocationsTable from '../components/tables/CouncilSeatAllocationsTable';
import VoteResultsTable from '../components/tables/VoteResultsTable';
import { AccentButton } from '../components/buttons/AccentButton';
import BoardSeatAllocationsTable from '../components/tables/BoardSeatAllocationsTable';
import TotalCouncilSeatsTable from '../components/tables/TotalCouncilSeatsTable';
import ElectionResultCard from '../components/cards/ElectionResultCard';

function ElectionResults() {

    const normalizeErrors = NormalizeErrors;
    const [allPoliticalParties, setAllPoliticalParties] = useState([]);
    const [electionYears, setElectionYears] = useState([]);
    const [allElectionResults, setAllElectionResults] = useState([]);
    const [allocatedResults, setAllocatedResults] = useState(null);
    const [showBoardSeatAllocations, setShowBoardSeatAllocations] = useState(false);
    const [boardSeatAllocationSet, setBoardSeatAllocationSet] = useState(null);
    const [maxSeatCount, setMaxSeatCount] = useState('');

    const [originalElectionResults, setOriginalElectionResults] = useState({
        municipalityId: '',
        electionYearId: '',
        totalCouncilSeatCount: '',
        politicalParties: [],
        voteResults: []
    });


    useEffect(() => {
        const fetchAllPoliticalParties = async () => {
            try {
                var data = await ApiFetch(
                    `${API_URL}/politicalparty/get-all`,
                    { method: "GET" },
                    true
                );

                if (data && data.length > 0) {
                    const parliamentaryParties = data.filter(party => party.isParliamentary);
                    setOriginalElectionResults(prev => ({
                        ...prev,
                        politicalParties: prev.politicalParties.length > 0
                        ? prev.politicalParties
                        : parliamentaryParties.map(party => ({
                            id: party.id,
                            name: party.name,
                            isLocal: party.isLocal,
                            isNew: false,
                            municipalityId: party.municipalityId
                        }))
                    }));
                }
                setAllPoliticalParties(data);

            } catch (error) {
                console.error(error);
                if (error.errors) {
                    const normalizedErrors = normalizeErrors(error.errors);
                    console.log("Normalized errors:", normalizedErrors);
                }
            }
        };

        const fetchElectionYears = async () => {
            try {
                var data = await ApiFetch(
                    `${API_URL}/election/get-all`,
                    { method: "GET" },
                    true
                );                
                setElectionYears(data);

            } catch (error) {
                console.error(error);
                if (error.errors) {
                    const normalizedErrors = normalizeErrors(error.errors);
                    console.log("Normalized errors:", normalizedErrors);
                }
            }
        };

        const fetchAllElectionResults = async () => {
            try {
                const data = await ApiFetch(
                    `${API_URL}/election/get-all-election-results`,
                    { method: "GET" },
                    true
                );

                if (data && data.length > 0) {
                    setAllElectionResults(data);
                }
            } catch (error) {
                console.error(error);
                if (error.errors) {
                    const normalizedErrors = normalizeErrors(error.errors);
                    console.log("Normalized errors:", normalizedErrors);
                }
            }
        }

        fetchAllPoliticalParties();
        fetchElectionYears();
        fetchAllElectionResults();
    }, []);

    
    useEffect(() => {
        if (!showBoardSeatAllocations || !allocatedResults) 
            return;

        const fetchBoardSeatAllocations = async () => {
            if(allocatedResults.boardSeatAllocationSet) {
                setShowBoardSeatAllocations(true);
                setBoardSeatAllocationSet(allocatedResults.boardSeatAllocationSet);
                setMaxSeatCount(allocatedResults.boardSeatAllocationSet.maxSeatCount);
                return;
            }

            try {
                const data = await ApiFetch(
                    `${API_URL}/election/add-board-allocations`,
                    { 
                        method: "POST",
                        body: {
                            originalElectionResultSetId: allocatedResults.id,
                            maxSeatCount: maxSeatCount
                        }
                    },
                    true
                );

                if(!data)
                    setBoardSeatAllocationSet(null);
                else
                    setBoardSeatAllocationSet(data);
            } catch (error) {
                console.error(error);
                if (error.errors) {
                    const normalizedErrors = normalizeErrors(error.errors);
                    console.log("Normalized errors:", normalizedErrors);
                }
                setBoardSeatAllocationSet(null);
                setShowBoardSeatAllocations(false);
                setMaxSeatCount('');
            }   
        }

        fetchBoardSeatAllocations();
    }, [showBoardSeatAllocations, allocatedResults]);

  return (
    <>
        <div className="content-container">
            <h2 className="manrope-extra-bold">Valresultat</h2>
            <p>För tillfället går det bara att skapa scenario för kommuner med en valkrets.</p>

            <div className="content">
                {allocatedResults ? (
                    <>
                        <div className="input-group input-group-small-18">
                            {!allocatedResults.boardSeatAllocationSet && (
                                <>
                                    <label
                                        className="manrope-semibold"
                                        htmlFor="maxSeatCount"
                                        label="Mandat för nämnder/styrelser" >
                                        Mandat för nämnder/styrelser
                                    </label>
                                    <input                             
                                        name="maxSeatCount"
                                        id="maxSeatCount"
                                        className="small-width"
                                        type="number" 
                                        value={maxSeatCount} 
                                        placeholder='Ange max antal...'
                                        onChange={(e) => setMaxSeatCount(Number(e.target.value))} 
                                    />
                                    <AccentButton 
                                        btnText="Beräkna"
                                        onClick={() => setShowBoardSeatAllocations(true)} />
                                </>
                            )}
                            {((boardSeatAllocationSet && maxSeatCount) || (showBoardSeatAllocations && allocatedResults.boardSeatAllocationSet)) && (
                                <BoardSeatAllocationsTable boardSeatAllocationSet={boardSeatAllocationSet} maxSeatCount={maxSeatCount} />
                            )}
                        </div>
                        <div className="result-tables-container">
                            <CouncilSeatAllocationsTable originalAllocationResults={allocatedResults} />

                            <aside className="result-tables-aside">
                                <VoteResultsTable originalAllocationResults={allocatedResults} />
                                <TotalCouncilSeatsTable originalAllocationResults={allocatedResults} />
                            </aside>
                        </div>                        
                    </>
                ) : (
                    <>
                        <div className="card-container">
                            {allElectionResults.length > 0 && (
                                allElectionResults.map(result => (
                                    <ElectionResultCard 
                                        key={`election-result-card-${result?.id}`}
                                        originalElectionResult={result}
                                        setAllocatedResults={setAllocatedResults}
                                        setBoardSeatAllocationSet={setBoardSeatAllocationSet}
                                        setShowBoardSeatAllocations={setShowBoardSeatAllocations}
                                    />
                                ))
                            )}
                        </div>
                        <h2>Lägg till nytt valresultat</h2>
                        <AddElectionResultForm 
                            setAllocatedResults={setAllocatedResults}
                            originalElectionResults={originalElectionResults} 
                            setOriginalElectionResults={setOriginalElectionResults} 
                            allPoliticalParties={allPoliticalParties}
                            electionYears={electionYears} />
                    </>
                )}
            </div>
        </div>
    </>
  )
}
export default ElectionResults;