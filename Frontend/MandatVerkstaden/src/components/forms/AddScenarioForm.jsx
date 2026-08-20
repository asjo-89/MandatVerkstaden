import { useState } from 'react'
import { ConfirmButton } from '../buttons/ConfirmButton'
import { Input } from '../inputs/Input'
import { Select } from '../inputs/Select'

const AddScenarioForm = ({ municipalities, formData, setFormData }) => {

    const [isChangeParties, setIsChangeParties] = useState(false);

    


  return (
    <form>
        <Input 
            type="text"
            id="scenarioName"
            name="scenarioName"
            htmlFor="scenarioName"
            placeholder="Ex. Scenario 1"
            onChange={(e) => setFormData({ ...formData, scenarioName: e.target.value })}
            value={formData.scenarioName}
        />
        <Select
            id="electionAreaName"
            name="electionAreaName"
            htmlFor="electionAreaName"
            label="Välj kommun"
            value={formData.electionAreaName}
            onChange={(e) => setFormData({ ...formData, electionAreaName: e.target.value })}
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
            value={formData.electionYear}
            onChange={(e) => setFormData({ ...formData, electionYear: e.target.value })}
            defaultOptValue="Välj valår"
            options={[
                { value: "2022", label: "2022" },
                { value: "2018", label: "2018" }
            ]}
        />
        <Input
            type="number"
            id="councilSeatCount"
            name="councilSeatCount"
            htmlFor="councilSeatCount"
            label="Mandat"
            placeholder="Ex. 31"
            onChange={(e) => setFormData({ ...formData, councilSeatCount: e.target.value })}
            value={formData.councilSeatCount}
        />
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
        {/* *********************************************** */}
            {/* Om isChangeParties är "Ja", 
                    visa en lista med partier som kan tas bort och ett formulär för att lägga till nya partier. 
                Om isChangeParties är "Nej", 
                    visa en lista med valda partier. 
                    
                    Formuläret skickas till /PoliticalParty/AddPoliticalParty */}
                    {isChangeParties && (
                        <form className="selected-parties-container">
                            <div className="input-group">
                                <label htmlFor="partyName" className="manrope-semibold">Partinamn</label>
                                <input type="text" id="partyName" name="partyName" placeholder="Ex. Parti 1" 
                                    onChange={(e) => setFormData({ ...formData, partyName: e.target.value })} value={formData.partyName} />
                            </div>
                            <div className="input-group">
                                <label htmlFor="partyName" className="manrope-semibold">Är det ett lokalt parti?</label>
                                <select id="isLocal" name="isLocal" value={formData.isLocal} 
                                    onChange={(e) => setFormData({...formData, isLocal: e.target.value === 'true'})}>

                                    <option value="">Välj</option>
                                    <option value="true">Ja</option>
                                    <option value="false">Nej</option>
                                </select>
                            </div>
                            <button className="button button-primary manrope-bold" type="submit">Lägg till parti</button>
                        </form>
                    )}
        {/* ************************************************* */}
        <ConfirmButton btnType="submit" className="btn-primary" btnText="Skapa scenario" />
    </form>
  )
}
export default AddScenarioForm;