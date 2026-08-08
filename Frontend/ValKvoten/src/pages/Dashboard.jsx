import { useState } from 'react';

function Dashboard() {
  const [isLocal, setIsLocal] = useState(''); 
  const [isChangeParties, setIsChangeParties] = useState('');
  const [isMultipleElectorialDistricts, setIsMultipleElectorialDistricts] = useState('');

  return (
    <>
        <div className="content-container">
            <h2 className="manrope-extra-bold">Skapa ett scenario</h2>
            <div className="form-container">
                <p className="form-description">Med ett scenario kan du laborera med olika valresultat och se hur de påverkar utfallet.</p>
                
    {/* Lägga till ett scenario */}
                <form>
                    <div className="input-group">
                        <label htmlFor="hasMultipleElectorialDistricts" className="manrope-semibold">Har kommunen flera valkretsar?</label>
                        <select id="hasMultipleElectorialDistricts" name="isMultipleElectorialDistricts" value={isMultipleElectorialDistricts} onChange={(e) => setIsMultipleElectorialDistricts(e.target.value)}>
                            <option value="">Välj</option>
                            <option value="yes">Ja</option>
                            <option value="no">Nej</option>
                        </select>
                    </div>
                    {isMultipleElectorialDistricts === 'yes' && (
                        <p className="manrope-semibold alert-danger">Tyvärr går det inte att göra beräkningar för kommuner med flera valkretsar i denna version av ValKvoten.</p>
                    )}
                    {isMultipleElectorialDistricts === 'no' && (
                        <>
                            <p className="manrope-medium alert-info">Som standard finns de etablerade rikspartierna samt ett urval av övriga partier som normalt redovisas i valresultat. Lokala partier och andra partier som inte finns med från början kan enkelt läggas till manuellt innan mandatfördelningen beräknas.</p>
                            
                            <div className="row-group">
                                <div className="input-group input-group-medium">
                                    <label htmlFor="scenarioName" className="manrope-semibold">Namn</label>
                                    <input type="text" id="scenarioName" name="scenarioName" placeholder="Ex. Scenario 1" />
                                </div>
                                <div className="input-group input-group-small">
                                    <label htmlFor="election" className="manrope-semibold">Valår</label>
                                    <select id="election" name="election">
                                        <option value="" disabled>Välj år</option>
                                        <option value="val1">Val 1</option>
                                        <option value="val2">Val 2</option>
                                    </select>
                                </div>
                            </div>
                            <div className="row-group">
                                <div className="input-group input-group-medium">
                                    <label htmlFor="municipality" className="manrope-semibold">Välj kommun</label>
                                    <select id="municipality" name="municipality">
                                        <option value="" disabled>Välj kommun</option>
                                        <option value="kommun1">Kommun 1</option>
                                        <option value="kommun2">Kommun 2</option>
                                    </select>
                                </div>
                                <div className="input-group input-group-small">
                                    <label htmlFor="councilSeatCount" className="manrope-semibold">Mandat</label>
                                    <input type="number" id="councilSeatCount" name="councilSeatCount" placeholder="Ex. 31" />
                                </div>
                            </div>
                                                                                    
                            <div className="input-group">
                                <label htmlFor="isLocal" className="manrope-semibold">Vill du ändra listan med partier?</label>
                                <select id="isLocal" name="isLocal" value={isChangeParties} onChange={(e) => setIsChangeParties(e.target.value)}>
                                    <option value="">Välj</option>
                                    <option value="yes">Ja</option>
                                    <option value="no">Nej</option>
                                </select>
                            </div>
                            <div className="column-group">
                                <p className="manrope-semibold">Valda partier:</p>
                                <div className="column-group">
                                    <div className="added-parties-list">                                        
                                        <div className="added-party-item">
                                            <p>Socialdemokraterna</p>                                            
                                            {isChangeParties === 'yes' && (
                                                <button type="button" className="manrope-bold">X</button>
                                            )}
                                        </div>
                                        <div className="added-party-item">
                                            <p>Kommunens bästa</p>
                                            {isChangeParties === 'yes' && (
                                                <button type="button" className="manrope-bold">X</button>
                                            )}
                                        </div>
                                        <div className="added-party-item">
                                            <p>Sjukvårdspartiet</p>
                                            {isChangeParties === 'yes' && (
                                                <button type="button" className="manrope-bold">X</button>
                                            )}
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div className="column-group">
                                <div className="input-group">
                                    <label htmlFor="partyName" className="manrope-semibold">Namnge partiet</label>
                                    <input type="text" id="partyName" name="partyName" placeholder="Ex. Parti 1" />
                                </div>
                                <div className="row-group">
                                    <div className="input-group">
                                        <label htmlFor="isLocal" className="manrope-semibold">Är det ett lokalt parti?</label>
                                        <select id="isLocal" name="isLocal" value={isLocal} onChange={(e) => setIsLocal(e.target.value)}>
                                            <option value="">Välj</option>
                                            <option value="yes">Ja</option>
                                            <option value="no">Nej</option>
                                        </select>
                                    </div>
                                    <button className="button button-primary manrope-bold" type="button">Lägg till parti</button>
                                </div>
                            </div>
                            <div className="button-container">
                                <button className="button button-submit manrope-bold" type="submit">Skapa scenario</button>
                            </div>
                        </>
                    )}  
                </form>
            </div>
        </div>
    </>
  )
}

export default Dashboard