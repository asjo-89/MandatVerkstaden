// import AddScenarioForm from '../components/forms/AddScenarioForm';
import AddElectionResultForm from '../components/forms/AddElectionResultForm';
import API_URL from '../ApiUrl';
import NormalizeErrors from '../components/helpers/NormalizeErrors';
import { useEffect, useState } from 'react';
import { ApiFetch } from '../components/helpers/ApiFetch';
import ResultTable from '../components/tables/resultTable';

function ElectionResults() {

    const normalizeErrors = NormalizeErrors;
    const [allPoliticalParties, setAllPoliticalParties] = useState([]);
    const [electionYears, setElectionYears] = useState([]);
    const [allocatedResults, setAllocatedResults] = useState(null);

    const [originalElectionResults, setOriginalElectionResults] = useState({
        municipalityId: '',
        electionYearId: '',
        totalCouncilSeatCount: '',
        politicalParties: [],
        voteResults: [],
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

        fetchAllPoliticalParties();
        fetchElectionYears();
    }, []);


  return (
    <>
        <div className="content-container">
            <h2 className="manrope-extra-bold">Valresultat</h2>
            <p>För tillfället går det bara att skapa scenario för kommuner med en valkrets.</p>

            <div className="content">
                {allocatedResults && allocatedResults.length > 0 ? (
                    <ResultTable originalAllocationResults={allocatedResults} />
                    ) : (
                    <AddElectionResultForm 
                        setAllocatedResults={setAllocatedResults}
                        originalElectionResults={originalElectionResults} 
                        setOriginalElectionResults={setOriginalElectionResults} 
                        allPoliticalParties={allPoliticalParties}
                        electionYears={electionYears} />
                )}
            </div>
        </div>
    </>
  )
}
export default ElectionResults;