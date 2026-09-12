import { useState } from 'react'
import { ConfirmButton } from '../buttons/ConfirmButton'
import { Input } from '../inputs/Input'
import { Select } from '../inputs/Select'
import { DeleteButton } from '../buttons/DeleteButton'
import { ApiFetch } from '../helpers/ApiFetch'
import API_URL from '../../ApiUrl'
import NormalizeErrors from '../helpers/NormalizeErrors'

const AddElectionResultForm = ({ 
        originalElectionResults, 
        setOriginalElectionResults, 
        allPoliticalParties,
        electionYears,
        setAllocatedResults,
        setSelectedParties
    }) => {

const [selectedPartyId, setSelectedPartyId] = useState('');
const [electionYearChosen, setElectionYearChosen] = useState(false);
const [municipalities, setMunicipalities] = useState([]);
const [totalVotesCount, setTotalVotesCount] = useState(null);

const selectedPartyIds = new Set(originalElectionResults.politicalParties.map(party => String(party.id)));


const [newParty, setNewParty] = useState({
    name: "",
    isLocal: "",
    municipalityId: ""
});

const handleElectionYearChange = (e) => {
    const selectedYearId = e.target.value;
    console.log("Selected election year ID:", selectedYearId);
    if(selectedYearId === "") {
        setElectionYearChosen(false);
        setMunicipalities([]);
        return;
    }
    setOriginalElectionResults(prev => ({
        ...prev,
        electionYearId: selectedYearId
    }));
    setElectionYearChosen(true);
        const fetchMunicipalities = async () => {
            try {
                var data = await ApiFetch(
                    `${API_URL}/municipality/get-all-with-one-constituency/${selectedYearId}`,
                    { method: "GET" },
                    true
                );
                setMunicipalities(data);
                console.log("Fetched municipalities:", data);

            } catch (error) {
                console.error(error);
                if (error.errors) {
                    const normalizedErrors = NormalizeErrors(error.errors);
                    console.log("Normalized errors:", normalizedErrors);
                }
            }
        };
        fetchMunicipalities();
}

const handleMunicipalityChange = (e) => {
    const selectedMunicipalityId = e.target.value;
    console.log("Selected municipality ID:", selectedMunicipalityId);
    setOriginalElectionResults(prev => ({
        ...prev,
        municipalityId: selectedMunicipalityId
    }));

    const totalSeatCount = municipalities
        .find(municipality => String(municipality.id) === String(selectedMunicipalityId))
        ?.electionConstituencies?.[0]?.fixedSeatCount || 0;

    setOriginalElectionResults(prev => ({
        ...prev,
        totalCouncilSeatCount: Number(totalSeatCount)
    }));
};

const handleAddParty = () => {
    const partyToAdd = allPoliticalParties.find(party => 
        String(party.id) === String(selectedPartyId));
    
    if(!partyToAdd) return;

    setOriginalElectionResults(prev => ({
        ...prev,
        politicalParties: 
        prev.politicalParties.some(party => party.id === partyToAdd.id)
        ? prev.politicalParties
        : [...prev.politicalParties, 
            { 
                id: partyToAdd.id, 
                name: partyToAdd.name,
                isLocal: partyToAdd.isLocal,
                isNew: partyToAdd.isNew ?? false,
                municipalityId: partyToAdd.municipalityId
            }
        ]
    }));
    setSelectedPartyId('');
}

const handleAddNewParty = async () => {
    if(originalElectionResults.municipalityId === "" && newParty.isLocal === true) {
        alert("Du måste välja en kommun innan du skapar ett nytt lokalt parti.");
        return;
    }

    if (!newParty.name || newParty.isLocal === "") {
        alert("Fyll i alla fält för det nya partiet.");
        return;
    }

    const nameExists = allPoliticalParties.some(party => party.name.toLowerCase() === newParty.name.toLowerCase()) || originalElectionResults.politicalParties.some(party => party.name.toLowerCase() === newParty.name.toLowerCase());

    if(nameExists) {
        alert("Det finns redan ett parti med samma namn.");
        return;
    }

    const partyToSend = {
        ...newParty,
        municipalityId: newParty.isLocal === true ? originalElectionResults.municipalityId : null
    };

    const createdParty = await ApiFetch(
        `${API_URL}/politicalparty/add`,
        {
            method: "POST",
            body: partyToSend
        },
        true
    );
    setOriginalElectionResults(prev => ({
        ...prev,
        politicalParties: [...prev.politicalParties,
            {
                id: createdParty.id,
                name: createdParty.name
            }
        ]
    }));
    setNewParty({ name: "", isLocal: "", municipalityId: "" });
}

const handleDeleteParty = (partyId) => {
    setOriginalElectionResults(prev => ({
        ...prev,
        politicalParties: prev.politicalParties.filter(party => party.id !== partyId)
    }));
}

const handleVoteResultChange = (partyId) => (e) => {
    const newVoteResult = e.target.value;

    const constituencyId = municipalities
        .find(m => String(m.id) === String(originalElectionResults.municipalityId))?.electionConstituencies?.[0]?.id;

    if(constituencyId === undefined) {
        alert("Kunde inte hitta valkretsen för den valda kommunen.");
        return;
    }

    setOriginalElectionResults(prev => {
        const exists = prev.voteResults.some(vote => vote.politicalPartyId === partyId);
        const voteResults = exists
            ? prev.voteResults.map(vote => 
                vote.politicalPartyId === partyId 
                    ? {...vote, numberOfVotes: newVoteResult, electionConstituencyId: constituencyId}
                    : vote)
            : [...prev.voteResults, { politicalPartyId: partyId, numberOfVotes: newVoteResult, electionConstituencyId: constituencyId }]
        ;
        return { ...prev, voteResults };
    });

    setTotalVotesCount(originalElectionResults.voteResults.reduce((total, vote) => total + Number(vote.numberOfVotes || 0), 0));
}

const handleSubmit = async (e) => {
    e.preventDefault();
    
    if (!originalElectionResults.municipalityId || !originalElectionResults.electionYearId) {
        alert("Vänligen välj kommun och valår.");
        return;
    }

    if(originalElectionResults.politicalParties.length === 0) {
        alert("Vänligen lägg till minst ett parti.");
        return;
    }

    if(originalElectionResults.voteResults.length !== originalElectionResults.politicalParties.length) {
        alert("Vänligen ange röster för alla valda partier.");
        return;
    }
    
    const data = await ApiFetch(
        `${API_URL}/election/add-election-result`,
        {
            method: "POST",
            body: originalElectionResults
        },
        true
    );

    if (data.error) {
        alert("Ett fel inträffade vid sparandet av valresultatet.");
        return;
    }
    setAllocatedResults(data);   
    setSelectedParties(new Set(originalElectionResults.politicalParties.map(party => String(party.id))));
}

  return (
    <>
        <div className="medium-width">
            <form className="form-container" id="election-result-form" onSubmit={handleSubmit}>
                <div className="row-group">
                    <Select
                        id="electionYear"
                        name="electionYear"
                        htmlFor="electionYear"
                        label="Välj valår"
                        width="input-group-small"
                        value={originalElectionResults.electionYearId}
                        onChange={handleElectionYearChange}
                        defaultOptValue="Välj valår"
                        options={electionYears.map((year) => {
                            return {
                                value: year.id,
                                label: year.electionYear
                            }
                        })}
                    />
                    {electionYearChosen && (
                        <Select
                            id="electionAreaName"
                            name="electionAreaName"
                            htmlFor="electionAreaName"
                            label="Välj kommun"
                            width="input-group-medium"
                            value={originalElectionResults.municipalityId}
                            onChange={handleMunicipalityChange}
                            defaultOptValue="Välj kommun"
                            options={municipalities.map((municipality) => ({
                                value: municipality.id,
                                label: municipality.electionAreaName
                            }))} 
                        />
                    )}
                </div>
                {electionYearChosen && originalElectionResults.municipalityId && (
                <>
                    <div className="input-group input-group-small row-group">
                        <Select
                            id="addPoliticalParty"
                            name="addPoliticalParty"
                            htmlFor="addPoliticalParty"
                            label="Lägg till fler partier"
                            value={selectedPartyId}
                            onChange={(e) => {
                                setSelectedPartyId(e.target.value)}}
                            defaultOptValue="Välj parti"
                            options={allPoliticalParties.map((party) => {
                                return {
                                    value: party.id,
                                    label: party.name,
                                    disabled: selectedPartyIds.has(String(party.id))
                                }
                            })
                            }
                        />
                        <ConfirmButton 
                            btnType="button" 
                            className="btn-primary" 
                            btnText="+"
                            onClick={handleAddParty} />
                    </div>
                    <div className="table-container">
                        <table className="selected-parties-table">
                            <thead>
                                <tr>
                                    <th className="text-align-left">Parti</th>
                                    <th>Röster</th>
                                    <th></th>
                                </tr>
                            </thead>

                            <tbody>
                                {originalElectionResults.politicalParties.map(party => (
                                    <tr key={party.id}>
                                        <td className="text-align-left">{party.name}</td>
                                        <td>
                                            <Input
                                                id={`voteResult-${party.id}`}
                                                name={`voteResult-${party.id}`}
                                                htmlFor={`voteResult-${party.id}`}
                                                type="number"
                                                min="0"
                                                placeholder="0"
                                                value={originalElectionResults.voteResults.find(vote => vote.politicalPartyId === party.id)?.numberOfVotes ?? ""}
                                                onChange={handleVoteResultChange(party.id)}
                                                onWheel={(e) => e.target.blur()}
                                            />
                                        </td>  
                                        <td>
                                            <DeleteButton 
                                                btnText="X" 
                                                className="btn-small manrope-bold"
                                                onClick={() => handleDeleteParty(party.id)} 
                                            />
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
                )}
            </form>
            {electionYearChosen && originalElectionResults.municipalityId && (
            <>
                {/* Outside the form so these fields are never part of the scenario submission */}
                <p>Saknar du ditt parti? Lägg till det här:</p>
                <div className="row-group">
                    <Input 
                        id="newPartyName" 
                        name="newPartyName" 
                        htmlFor="newPartyName"
                        label="Partinamn"
                        type="text" 
                        placeholder="Ex. Parti 1" 
                        value={newParty.name}
                        onChange={(e) => setNewParty(prev => ({ ...prev, name: e.target.value }))}
                    />
                    <Select 
                        id="isLocal" 
                        htmlFor="isLocal"
                        name="isLocal" 
                        label="Är det ett lokalt parti?"
                        value={newParty.isLocal === undefined
                            ? ""
                            : newParty.isLocal
                        }
                        onChange={(e) => setNewParty(prev => ({ ...prev, isLocal: e.target.value === "true"}))}
                        options={[
                            { value: true, label: "Ja" },
                            { value: false, label: "Nej" }
                        ]}
                    />
                    <ConfirmButton 
                        btnType="button" 
                        className="btn-primary" 
                        btnText="+"
                        onClick={handleAddNewParty} />
                </div>
                <ConfirmButton 
                    btnType="submit" 
                    form="election-result-form"
                    className="btn-primary" 
                    btnText="Skapa scenario" />
            </>
            )}
        </div>
    </>
  )
}
export default AddElectionResultForm;