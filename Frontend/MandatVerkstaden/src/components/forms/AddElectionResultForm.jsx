import { useState } from 'react'
import { ConfirmButton } from '../buttons/ConfirmButton'
import { Input } from '../inputs/Input'
import { Select } from '../inputs/Select'

const AddElectionResultForm = ({ 
        municipalities, 
        originalElectionResults, 
        setOriginalElectionResults, 
        allPoliticalParties,
        electionYears
    }) => {

const [selectedPartyId, setSelectedPartyId] = useState('');
    
const selectedPartyIds = new Set(originalElectionResults.politicalParties.map(party => String(party.id)));

const handleAddParty = () => {
    const partyToAdd = allPoliticalParties.find(party => 
        String(party.id) === String(selectedPartyId));
    
    if(!partyToAdd) return;

    setOriginalElectionResults(prev => ({
        ...prev,
        politicalParties: 
            prev.politicalParties.some(party => party.id === partyToAdd.id)
            ? prev.politicalParties
            : [...prev.politicalParties, partyToAdd]
    }));
    setSelectedPartyId('');
}

  return (
    <>
        <div className="medium-width">
            <form className="form-container">
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
                        onChange={(e) => 
                            setOriginalElectionResults(prev => ({
                                ...prev,
                                totalCouncilSeatCount: e.target.value
                            }))}
                        value={originalElectionResults.totalCouncilSeatCount}
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
                    
                <p>Saknar du ditt parti? Lägg till det här:</p>
                <div className="row-group">
                    <Input 
                        id="newPartyName" 
                        name="newPartyName" 
                        htmlFor="newPartyName"
                        label="Partinamn"
                        type="text" 
                        placeholder="Ex. Parti 1" 
                        value={originalElectionResults.newPoliticalParties.name || ""}
                        onChange={(e) => setOriginalElectionResults(prev => ({
                            ...prev, 
                            newPoliticalParties: { ...prev.newPoliticalParties, name: e.target.value } 
                        }))} 
                    />
                    <Select 
                        id="isLocal" 
                        htmlFor="isLocal"
                        name="isLocal" 
                        label="Är det ett lokalt parti?"
                        value={originalElectionResults.newPoliticalParties.isLocal === undefined
                            ? ""
                            : String(originalElectionResults.newPoliticalParties.isLocal)
                        }
                        onChange={(e) => setOriginalElectionResults(prev => ({
                            ...prev, 
                            newPoliticalParties: { ...prev.newPoliticalParties, isLocal: e.target.value === 'true' } 
                        }))}
                        options={[
                            { value: "true", label: "Ja" },
                            { value: "false", label: "Nej" }
                        ]}
                    />
                    <ConfirmButton 
                        btnType="button" 
                        className="btn-primary" 
                        btnText="+"
                        onClick={handleAddParty} />
                </div>
                <ConfirmButton btnType="submit" className="btn-primary" btnText="Skapa scenario" />
            </form>
        
        </div>

        <div className="selected-parties-container small-width">
            <h3 className="manrope-extra-bold">Valda partier</h3>
            <div className="selected-parties-list">
                {originalElectionResults.politicalParties.map(party => (
                    <div className="selected-parties-item" key={party.id}>
                        <p className="manrope-semibold">{party.name}</p>
                        <button type="button" className="manrope-bold">X</button>
                    </div>
                ))}
            </div>
        </div>
    </>
  )
}
export default AddElectionResultForm;