// import AddScenarioForm from '../components/forms/AddScenarioForm';
import AddElectionResultForm from '../components/forms/AddElectionResultForm';
import API_URL from '../ApiUrl';
import NormalizeErrors from '../components/helpers/NormalizeErrors';
import { useEffect, useState } from 'react';
import { ApiFetch } from '../components/helpers/ApiFetch';

function Scenario() {

    const normalizeErrors = NormalizeErrors;

    const [municipalities, setMunicipalities] = useState([]);

    const electionYears = [
        { id: 1, electionYear: "2026" },
        { id: 2, electionYear: "2022" },
        { id: 3, electionYear: "2018" }
    ];


    const [originalElectionResults, setOriginalElectionResults] = useState({
        municipalityId: '',
        electionYearId: '',
        councilSeatCount: '',
        isLocal: false,
        politicalParties: [],
        electionResults: [],
        newPoliticalParties: []
    });

    useEffect(() => {
        const fetchMunicipalities = async () => {
            try {
                var data = await ApiFetch(
                    `${API_URL}/municipality/get-all`,
                    { method: "GET" },
                    true
                );
                setMunicipalities(data);

            } catch (error) {
                console.error(error);
                if (error.errors) {
                    const normalizedErrors = normalizeErrors(error.errors);
                    console.log("Normalized errors:", normalizedErrors);
                }
            }
        };

        const fetchParliamentaryParties = async () => {
            try {
                var data = await ApiFetch(
                    `${API_URL}/politicalparty/get-all-parliamentary`,
                    { method: "GET" },
                    true
                );
                setOriginalElectionResults(prevState => ({
                    ...prevState,
                    politicalParties: data
                }));
            } catch (error) {
                console.error(error);
                if (error.errors) {
                    const normalizedErrors = normalizeErrors(error.errors);
                    console.log("Normalized errors:", normalizedErrors);
                }
            }
        };

        fetchMunicipalities();
        fetchParliamentaryParties();
    }, []);

  return (
    <>
        <div className="content-container">
            <h2 className="manrope-extra-bold">Skapa ett scenario</h2>
            <p>För tillfället går det bara att skapa scenario för kommuner med en valkrets.</p>

            {/* <AddScenarioForm municipalities={municipalities} formData={formData} setFormData={setFormData} /> */}
            <AddElectionResultForm 
                municipalities={municipalities} 
                originalElectionResults={originalElectionResults} 
                setOriginalElectionResults={setOriginalElectionResults} 
                electionYears={electionYears} />


            {/* <h2>Lägg till valresultat</h2>
            <form>
                <div className="input-group">
                    <label htmlFor="socialdemokraterna">Socialdemokraterna</label>
                    <input type="number" id="socialdemokraterna" name="socialdemokraterna" />
                </div>
                <div className="input-group">
                    <label htmlFor="moderaterna">Moderaterna</label>
                    <input type="number" id="moderaterna" name="moderaterna" />
                </div>
                <div className="input-group">
                    <label htmlFor="sverigedemokraterna">Sverigedemokraterna</label>
                    <input type="number" id="sverigedemokraterna" name="sverigedemokraterna" />
                </div>
                <div className="input-group">
                    <label htmlFor="vansterpartiet">Vänsterpartiet</label>
                    <input type="number" id="vansterpartiet" name="vansterpartiet" />
                </div>
                <div className="input-group">
                    <label htmlFor="miljopartiet">Miljöpartiet</label>
                    <input type="number" id="miljopartiet" name="miljopartiet" />
                </div>
                <div className="input-group">
                    <label htmlFor="centerpartiet">Centerpartiet</label>
                    <input type="number" id="centerpartiet" name="centerpartiet" />
                </div>
                <div className="input-group">
                    <label htmlFor="kristdemokraterna">Kristdemokraterna</label>
                    <input type="number" id="kristdemokraterna" name="kristdemokraterna" />
                </div>
                <div className="input-group">
                    <label htmlFor="liberalerna">Liberalerna</label>
                    <input type="number" id="liberalerna" name="liberalerna" />
                </div>
                <div>
                    <p>Totalt antal röster: 0</p>
                </div>
                <button className="button button-primary manrope-bold" type="submit">Beräkna mandatfördelning</button>
            </form> */}
        </div>
    </>
  )
}
export default Scenario;