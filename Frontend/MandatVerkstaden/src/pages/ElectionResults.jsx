import { DraggablePartyCard } from '../components/DraggablePartyCard';
import AddElectionResultForm from '../components/forms/AddElectionResultForm';
import API_URL from '../ApiUrl';
import NormalizeErrors from '../components/helpers/NormalizeErrors';
import { useEffect, useState, useMemo } from 'react';
import { ApiFetch } from '../components/helpers/ApiFetch';
import CouncilSeatAllocationsTable from '../components/tables/CouncilSeatAllocationsTable';
import VoteResultsTable from '../components/tables/VoteResultsTable';
import { AccentButton } from '../components/buttons/AccentButton';
import BoardSeatAllocationsTable from '../components/tables/BoardSeatAllocationsTable';
import TotalCouncilSeatsTable from '../components/tables/TotalCouncilSeatsTable';
import ElectionResultCard from '../components/cards/ElectionResultCard';
import { Input } from '../components/inputs/Input';
import PartyGroupsBoardAllocationsTable, { createTieBreakers } from '../components/tables/PartyGroupsBoardAllocationsTable';
import VotesForLastMandateTable from '../components/tables/VotesForLastMandateTable';
import { DragDropProvider } from '@dnd-kit/react';
import { DroppablePartyCard } from '../components/DroppablePartyCard';
import { ConfirmButton } from '../components/buttons/ConfirmButton';
import { DeleteButton } from '../components/buttons/DeleteButton';
import { BiChevronDown, BiChevronUp } from 'react-icons/bi';

function ElectionResults() {

    const normalizeErrors = NormalizeErrors;
    const [allPoliticalParties, setAllPoliticalParties] = useState([]);
    const [electionYears, setElectionYears] = useState([]);
    const [allElectionResults, setAllElectionResults] = useState([]);
    const [allocatedResults, setAllocatedResults] = useState(null);
    const [showBoardSeatAllocations, setShowBoardSeatAllocations] = useState(false);
    const [boardSeatAllocationSet, setBoardSeatAllocationSet] = useState(null);
    const [maxSeatCount, setMaxSeatCount] = useState('');
    
    // const [isDropped, setIsDropped] = useState(false);
    const [isActiveDragId, setIsActiveDragId] = useState(null);
    const [groupAssignments, setGroupAssignments] = useState({});
    const [hideCountButton, setHideCountButton] = useState(false);

    // const [showCreatePartyGroup, setShowCreatePartyGroup] = useState(false);

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


    const [newGroupName, setNewGroupName] = useState('');
    const [partyGroups, setPartyGroups] = useState([])

    const partiesInCouncil = useMemo(() => getPartiesInCouncil(allocatedResults), [allocatedResults]);

    // Bygg om partyGroups till formen buildUnits/createTieBreakers förväntar sig,
    // baserat på nuvarande groupAssignments (dvs. var korten faktiskt är just nu).
    const partyGroupsWithMembers = useMemo(() => {
        return partyGroups.map(group => ({
            id: group.id,
            name: group.name,
            partyIds: new Set(
                partiesInCouncil
                    .filter(({ partyId }) => groupAssignments[partyId] === group.id)
                    .map(({ partyId }) => partyId)
            )
        }));
    }, [partyGroups, groupAssignments, partiesInCouncil]);

    // Räknas om varje gång grupperingen ändras, så eventuell lottning speglar
    // den senaste grupperingen.
    const boardTieBreakers = useMemo(() => {
        if (!allocatedResults) return {};
        return createTieBreakers(
            allocatedResults.seatAllocations,
            partyGroupsWithMembers,
            maxSeatCount
        );
    }, [allocatedResults, partyGroupsWithMembers, maxSeatCount]);

    function getPartiesInCouncil(allocations) {
        const parties = new Map();

        for (const allocation of allocations?.seatAllocations ?? []) {
            parties.set(String(allocation.politicalPartyId), {
                partyId: String(allocation.politicalPartyId),
                partyName: allocation.politicalPartyName,
                totalSeats: allocation.totalCouncilSeatCountForParty
            });
        }

        return [...parties.values()];
    }

    function handleCreatePartyGroup(e) {
        e.preventDefault();
        document.getElementById('create-party-group-dialog').close();
        
        if(!newGroupName)
            return;

        if(partyGroups.some(group => group.name === newGroupName)) {
            alert('Det finns redan en grupp med det namnet.');
            return;
        }

        const newGroupId = `group-${Date.now()}-${Math.random().toString(36).slice(2, 8)}`;

        setPartyGroups(prev => [...prev, { id: newGroupId, name: newGroupName }]);
        setNewGroupName('');
    }

    function handleDeletePartyGroup(groupId) {
        setPartyGroups(prev => prev.filter(group => group.id !== groupId));

        setGroupAssignments(prev => {
            const updated = { ...prev };
            for (const partyId of Object.keys(updated)) {
                if (updated[partyId] === groupId) {
                    delete updated[partyId];
                }
            }
            return updated;
        });
    }

  return (
    <>
        <div className="content-container">
            <h2 className="manrope-extra-bold">Valresultat</h2>
            <p>För tillfället går det bara att skapa scenario för kommuner med en valkrets.</p>

            <div className="content">
                {allocatedResults ? (
                    <>
                        <div className="allocations-container">
                            {(!allocatedResults.boardSeatAllocationSet || showBoardSeatAllocations) && (
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
                                        value={(maxSeatCount > 0 ? maxSeatCount : '')} 
                                        placeholder='Ange max antal...'
                                        onChange={(e) => setMaxSeatCount(Number(e.target.value))} 
                                    />
                                    {!hideCountButton && (
                                        <AccentButton 
                                            btnText="Beräkna"
                                            onClick={() => {
                                                setShowBoardSeatAllocations(true)
                                                setHideCountButton(true)
                                            }} />
                                    )}
                                </>
                            )}
                            {((boardSeatAllocationSet && maxSeatCount) || (showBoardSeatAllocations && allocatedResults.boardSeatAllocationSet)) && (     
                                
                                <div className="party-group-creation">
                                    <div className="party-groups-board-allocations-container">
                                        <h3>Mandat i nämnder/styrelser</h3>
                                        <PartyGroupsBoardAllocationsTable
                                            originalCouncilSeatAllocations={allocatedResults.seatAllocations} 
                                            maxSeatCount={maxSeatCount} 
                                            partyGroups={partyGroupsWithMembers}
                                            tieBreakers={boardTieBreakers}
                                        />
                                    </div> 
                                    <div className="party-group-creation-header">
                                        <h3>Konfigurera partigrupper</h3>
                                        <button className="toggle-button" onClick={() => setShowBoardSeatAllocations(prev => !prev)}>
                                            {showBoardSeatAllocations ? <BiChevronDown /> : <BiChevronUp />}
                                        </button>
                                    </div>
                                    {showBoardSeatAllocations && (
                                        <DragDropProvider
                                            onDragStart={(event) => {
                                                const id = event.operation.source?.id;
                                                setIsActiveDragId(id ? String(id).replace("groupId-", "") : null);
                                            }}
                                            onDragEnd={(event) => {
                                                setIsActiveDragId(null);
                                                if(event.canceled) return;

                                                const sourceId = event.operation.source?.id;
                                                const targetId = event.operation.target?.id;
                                                if(!sourceId || !targetId) return;  

                                                const partyId = String(sourceId).replace("partyId-", "");
                                                const groupId = String(targetId).replace("groupId-", "");

                                                setGroupAssignments(prev => ({
                                                    ...prev,
                                                    [partyId]: groupId === "unassigned" ? undefined : groupId
                                                }));
                                            }}>
                                            <div className="party-assignment-container">
                                                <DroppablePartyCard groupName="Ej grupperade partier" groupId="unassigned">
                                                    <div className="party-card-container">
                                                        {partiesInCouncil
                                                            .filter(({ partyId }) => !groupAssignments[partyId])
                                                            .map(({ partyName, partyId, totalSeats }) => (
                                                                <DraggablePartyCard key={partyId} partyName={partyName} partyId={partyId} totalSeats={totalSeats} />
                                                            ))}
                                                    </div>
                                                </DroppablePartyCard>
                                                
                                                <div className="party-groups-container">
                                                    <div className="party-groups-top">
                                                        <h3>Partigrupper</h3>
                                                        <ConfirmButton
                                                            btnText="Skapa partigrupp"
                                                            onClick={() => document.getElementById('create-party-group-dialog').showModal()} />
                                                        
                                                        <dialog id="create-party-group-dialog">
                                                            <form>
                                                                <div className="dialog-top">
                                                                    <h2>Skapa partigrupp</h2>
                                                                    <DeleteButton
                                                                        className="dialog-close-button"
                                                                        btnText="X"
                                                                        onClick={() => document.getElementById('create-party-group-dialog').close()}
                                                                    />
                                                                </div>
                                                                <Input
                                                                    name="partyGroupName"
                                                                    id="partyGroupName"
                                                                    className="small-width"
                                                                    type="text" 
                                                                    value={newGroupName}
                                                                    placeholder='Ange partigruppens namn...'
                                                                    onChange={e => setNewGroupName(e.target.value)}
                                                                />
                                                                <ConfirmButton
                                                                    className="confirm-dialog-button"
                                                                    btnText="Skapa"
                                                                    onClick={handleCreatePartyGroup}
                                                                />
                                                            </form>
                                                        </dialog>
                                                    </div>
                                                    <div className="party-groups-cards">
                                                    {partyGroups.map(group => (
                                                        <DroppablePartyCard
                                                            key={group.id}
                                                            groupName={group.name}
                                                            groupId={group.id}
                                                            onDelete={handleDeletePartyGroup}
                                                        >
                                                            {partiesInCouncil
                                                                .filter(({ partyId }) => groupAssignments[partyId] === group.id)
                                                                .map(({ partyName, partyId, totalSeats }) => (
                                                                    <DraggablePartyCard key={partyId} partyName={partyName} partyId={partyId} totalSeats={totalSeats} /> 
                                                                ))}
                                                        </DroppablePartyCard>
                                                    ))}
                                                    </div>
                                                </div>                                            
                                            </div>
                                        </DragDropProvider>    
                                    )}                                                                
                                </div>
                            )}
                        </div>
                        <div className="result-tables-container">
                            <div>
                                <h3>Mandatfördelning i KF</h3>
                                <CouncilSeatAllocationsTable originalAllocationResults={allocatedResults} />
                            </div>
                            <aside className="result-tables-aside">
                                <div>
                                    <h3>Valresultat</h3>
                                    <VoteResultsTable originalAllocationResults={allocatedResults} />
                                </div>
                                <div style={{display: "flex", flexDirection: "row", gap: "2rem"}}>
                                    <div>
                                        <h3>Totala mandat i KF</h3>
                                        <TotalCouncilSeatsTable originalAllocationResults={allocatedResults} />
                                    </div>
                                    <div>
                                        <h3>Röster till sista mandatet</h3>
                                        <VotesForLastMandateTable originalAllocationResults={allocatedResults} />
                                    </div> 
                                </div>
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
