import { useState } from 'react'
import { ConfirmButton } from '../buttons/ConfirmButton'
import { Input } from '../inputs/Input'
import { Select } from '../inputs/Select'
import { DeleteButton } from '../buttons/DeleteButton'
import { ApiFetch } from '../helpers/ApiFetch'
import API_URL from '../../ApiUrl'

const AddElectionResultForm = ({ 
        municipalities, 
        originalElectionResults, 
        setOriginalElectionResults, 
        allPoliticalParties,
        electionYears
    }) => {

const [selectedPartyId, setSelectedPartyId] = useState('');
const selectedPartyIds = new Set(originalElectionResults.politicalParties.map(party => String(party.id)));

const [newParty, setNewParty] = useState({
    name: "",
    isLocal: "",
    municipalityId: ""
});

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
    if(originalElectionResults.municipalityId === "" && newParty.isLocal === "true") {
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

    await ApiFetch(
        `${API_URL}/politicalparty/add`,
        {
            method: "POST",
            body: originalElectionResults
        },
        true
    );
    setOriginalElectionResults(prev => ({
        ...prev,
        politicalParties: [...prev.politicalParties,
            {
                id: newParty.id,
                name: newParty.name
            }
        ]
    }));
}

const handleDeleteParty = (partyId) => {
    setOriginalElectionResults(prev => ({
        ...prev,
        politicalParties: prev.politicalParties.filter(party => party.id !== partyId)
    }));
}

const handleVoteResultChange = (partyId) => (e) => {
    const newVoteResult = e.target.value;
    setOriginalElectionResults(prev => {
        const exists = prev.voteResults.some(vote => vote.partyId === partyId);
        const voteResults = exists
            ? prev.voteResults.map(vote => 
                vote.partyId === partyId 
                    ? {...vote, numberOfVotes: newVoteResult}
                    : vote)
            : [...prev.voteResults, { partyId, numberOfVotes: newVoteResult }]
        ;
        return { ...prev, voteResults };
    });
}

const handleSubmit = (e) => {
    e.preventDefault();
    
    
}

  return (
    <>
        <div className="medium-width">
            <form className="form-container" id="election-result-form" onSubmit={handleSubmit}>
                <div className="row-group">
                    <Select
                        id="electionAreaName"
                        name="electionAreaName"
                        htmlFor="electionAreaName"
                        label="Välj kommun"
                        width="input-group-medium"
                        value={originalElectionResults.municipalityId}
                        onChange={(e) => setOriginalElectionResults(prev => ({ 
                            ...prev, municipalityId: e.target.value }))}
                        defaultOptValue="Välj kommun"
                        options={municipalities.map((municipality) => ({
                            value: municipality.id,
                            label: municipality.electionAreaName
                        }))} 
                    />
                    <Select
                        id="electionYear"
                        name="electionYear"
                        htmlFor="electionYear"
                        label="Välj valår"
                        width="input-group-small"
                        value={originalElectionResults.electionYearId}
                        onChange={(e) => setOriginalElectionResults(prev => ({
                            ...prev,
                            electionYearId: e.target.value
                        }))}
                        defaultOptValue="Välj valår"
                        options={electionYears.map((year) => {
                            return {
                                value: year.id,
                                label: year.electionYear
                            }
                        })}
                    />
                    <Input
                        id="totalCouncilSeatCount"
                        name="totalCouncilSeatCount"
                        htmlFor="totalCouncilSeatCount"
                        type="number"
                        label="Mandat"
                        width="input-group-small"
                        placeholder="Ex. 31"
                        value={originalElectionResults.totalCouncilSeatCount}
                        onChange={(e) => 
                            setOriginalElectionResults(prev => ({ ...prev, totalCouncilSeatCount: e.target.value }))}
                    />
                </div>
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
            </form>

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
                        : String(newParty.isLocal)
                    }
                    onChange={(e) => setNewParty(prev => ({ ...prev, isLocal: e.target.value}))}
                    options={[
                        { value: "true", label: "Ja" },
                        { value: "false", label: "Nej" }
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
        </div>

        <div className="selected-parties-container small-width">
            <h3 className="manrope-extra-bold">Valda partier</h3>
            <div className="selected-parties-list">
                {originalElectionResults.politicalParties.map(party => (
                    <div className="row-group selected-party-item" key={party.id}>
                        <p className="manrope-semibold">{party.name}</p>
                        <DeleteButton 
                            btnText="X" 
                            className="btn-small manrope-bold" 
                            onClick={() => handleDeleteParty(party.id)} />
                    </div>
                ))}
            </div>
            <table className="selected-parties-table">
                <thead>
                    <tr>
                        <th>Parti</th>
                        <th>Röster</th>
                    </tr>
                </thead>

                <tbody>
                    {originalElectionResults.politicalParties.map(party => (
                        <tr key={party.id}>
                            <td>{party.name}</td>
                            <td>
                                <Input
                                    id={`voteResult-${party.id}`}
                                    name={`voteResult-${party.id}`}
                                    htmlFor={`voteResult-${party.id}`}
                                    type="number"
                                    placeholder="0"
                                    value={party.voteResult || ""}
                                    onChange={handleVoteResultChange(party.id)}
                                />
                            </td>   
                        </tr>
                    ))}
                </tbody>
            </table>
        </div>
    </>
  )
}
export default AddElectionResultForm;