import { useState } from 'react'
import { ConfirmButton } from '../buttons/ConfirmButton'
import { Input } from '../inputs/Input'
import { Select } from '../inputs/Select'

const AddElectionResultForm = ({ 
        municipalities, 
        originalElectionResults, 
        setOriginalElectionResults, 
        electionYears
    }) => {

    const [isChangeParties, setIsChangeParties] = useState(false);

    


  return (
    <form className="form-container">
        <div className="row-group">
            <Select
                id="electionAreaName"
                name="electionAreaName"
                htmlFor="electionAreaName"
                label="Välj kommun"
                width="input-group-medium"
                value={originalElectionResults.municipalityId}
                onChange={(e) => setOriginalElectionResults({ ...originalElectionResults, municipalityId: e.target.value })}
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
                onChange={(e) => setOriginalElectionResults({ ...originalElectionResults, electionYearId: e.target.value })}
                defaultOptValue="Välj valår"
                options={electionYears.map((year) => {
                    return {
                        value: year.id,
                        label: year.electionYear
                    }
                })}
            />
            <Input
                id="councilSeatCount"
                name="councilSeatCount"
                htmlFor="councilSeatCount"
                type="number"
                label="Mandat"
                width="input-group-small"
                placeholder="Ex. 31"
                onChange={(e) => setOriginalElectionResults({ ...originalElectionResults, councilSeatCount: e.target.value })}
                value={originalElectionResults.councilSeatCount}
            />
        </div>
        <div className="row-group">    
            <div className="input-group input-group-small">
                <Select
                    id="isChangeParties"
                    name="isChangeParties"
                    htmlFor="isChangeParties"
                    label="Vill du lägga till fler partier?"
                    value={isChangeParties}
                    onChange={(e) => setIsChangeParties(e.target.value === 'true')}
                    defaultOptValue="Välj"
                    options={[
                        { value: "true", label: "Ja" },
                        { value: "false", label: "Nej" }
                    ]}
                />
                {isChangeParties && (
                    <>
                            <Input 
                                id="name" 
                                name="name" 
                                htmlFor="name"
                                label="Partinamn"
                                type="text" 
                                placeholder="Ex. Parti 1" 
                                value={originalElectionResults.newPoliticalParties.name} 
                                onChange={(e) => setOriginalElectionResults({ ...originalElectionResults, 
                                    newPoliticalParties: { ...originalElectionResults.newPoliticalParties, name: e.target.value } })} 
                            />
                            <Select 
                                id="isLocal" 
                                htmlFor="isLocal"
                                name="isLocal" 
                                label="Är det ett lokalt parti?"
                                value={originalElectionResults.newPoliticalParties.isLocal} 
                                onChange={(e) => setOriginalElectionResults({ ...originalElectionResults, 
                                    newPoliticalParties: { ...originalElectionResults.newPoliticalParties, isLocal: e.target.value === 'true' } })}
                                options={[
                                    { value: "true", label: "Ja" },
                                    { value: "false", label: "Nej" }
                                ]}
                            />
                        <ConfirmButton btnType="button" className="btn-primary" btnText="Lägg till parti" />
                    </>
                )}
            </div>
            
            <div className="selected-parties-container medium-width">
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
        </div>
        <ConfirmButton btnType="submit" className="btn-primary" btnText="Skapa scenario" />
    </form>
  )
}
export default AddElectionResultForm;