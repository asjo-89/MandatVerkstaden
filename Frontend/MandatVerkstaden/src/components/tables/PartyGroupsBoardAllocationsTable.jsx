import { useMemo } from 'react';

// originalCouncilSeatAllocations: rådata från KF-fördelningen, en rad per parti/steg,
//         med minst { politicalPartyId, politicalPartyName, totalCouncilSeatCountForParty }.
// partyGroups: [{ name, partyIds: Set<string> }] - byggs upp i ElectionResults.jsx.
//         Partier som inte ingår i någon grupp behandlas som egna, ensamma enheter.
// maxSeatCount: max antal mandat att räkna fram (samma övre gräns som för original-nämnderna).
const PartyGroupsBoardAllocationsTable = ({ originalCouncilSeatAllocations, maxSeatCount, partyGroups, tieBreakers }) => {
    const maxBoardSeats = maxSeatCount ?? 0;

    // Bygg enheter (grupper + ensamma partier) med summerat mandatunderlag från KF
    const units = useMemo(
        () => buildUnits(originalCouncilSeatAllocations ?? [], partyGroups ?? []),
        [originalCouncilSeatAllocations, partyGroups]
    );

    // Kör d'Hondt-beräkningen på enheterna, memoized så resultatet (inkl. ev. lottning) inte räknas om vid varje render
    const { results: groupAllocations, comparisonMatrix } = useMemo(
        () => calculateBoardSeatAllocations(units, maxBoardSeats, tieBreakers),
        [units, maxBoardSeats, tieBreakers]
    );

    const oddBoardSeats = getOddBoardSeats(maxBoardSeats);

    const cumulativeSeatsListPerUnit = createCumulativeSeatCountPerStep(groupAllocations, maxBoardSeats);

    const sortedUnits = [...units].sort((a, b) => a.name.localeCompare(b.name, 'sv'));

    // Filtrerad lista med alla lottningsrader
    const lotDrawingInfo = groupAllocations
        .filter(allocation => allocation.lotDrawingGroupId)
        .sort((a, b) => a.seatAllocationStep - b.seatAllocationStep);

    // UnitId -> array av lottningshändelser
    const lotEventsForUnit = new Map();
    for (const row of lotDrawingInfo) {
        if (!lotEventsForUnit.has(row.unitId)) {
            lotEventsForUnit.set(row.unitId, []);
        }
        lotEventsForUnit.get(row.unitId).push({ step: row.seatAllocationStep, won: row.wonByLotDrawing });
    }

    function getTableCellContentWithLotEvents(step, unitId) {
        const currentSeats = cumulativeSeatsListPerUnit.get(step)?.get(unitId) ?? 0;
        const unitEvents = lotEventsForUnit.get(unitId)?.filter(event => event.step === step);

        if (!unitEvents || unitEvents.length === 0) {
            return { currentSeats, inLotDrawing: false };
        }

        const seatDifference = unitEvents.reduce((sum, event) => sum + (event.won ? -1 : 1), 0);
        const alternativeSeats = currentSeats + seatDifference;
        const [lower, higher] = [currentSeats, alternativeSeats].sort((a, b) => a - b);

        return { currentSeats, inLotDrawing: true, lower, higher, won: unitEvents.some(event => event.won) };
    }

    if (maxBoardSeats <= 0 || units.length === 0) {
        return null;
    }

    return (
        <div className="table-container">
            <table className="selected-parties-table">
                <thead>
                    <tr className="manrope-bold">
                        <th className="text-align-left">Partigrupp</th>
                        <th className="text-align-right">Mandat i KF</th>
                        {oddBoardSeats.map((seat) => (
                            <th key={seat}>{seat}-nämnd</th>
                        ))}
                    </tr>
                </thead>
                <tbody>
                    {sortedUnits.map((unit) => (
                        <tr key={unit.id}>
                            <td className="text-align-left" title={unit.partyNames.join('\n')}>
                                <div>{unit.name}</div>
                            </td>
                            <td className="text-align-right">{unit.totalCouncilSeatCountForParty}</td>
                            {oddBoardSeats.map((seat) => {
                                const cellInfo = getTableCellContentWithLotEvents(seat, unit.id);

                                if (!cellInfo.inLotDrawing) {
                                    return (
                                        <td key={seat}>
                                            <div>{cellInfo.currentSeats || ''}</div>
                                        </td>
                                    );
                                }

                                return (
                                    <td key={seat} className={`lot-drawing ${cellInfo.won ? 'lot-drawing-won' : ''}`}>
                                        <div>{cellInfo.lower} eller {cellInfo.higher}</div>
                                    </td>
                                );
                            })}
                        </tr>
                    ))}
                </tbody>
            </table>
        </div>
    );
};

// --- Hjälpfunktioner ---

// Bygger enhetslistan: en post per partigrupp (summerade mandat) + en post per ensamt/ogrupperat parti
function buildUnits(councilAllocations, partyGroups) {
    // Deduplicera: en rad per parti, med partiets totala KF-mandat (samma logik som backendens GroupBy/MaxBy)
    const totalSeatsByPartyId = new Map();
    for (const allocation of councilAllocations) {
        const partyIdKey = String(allocation.politicalPartyId);
        const existing = totalSeatsByPartyId.get(partyIdKey);
        if (!existing || allocation.totalCouncilSeatCountForParty > existing.totalCouncilSeatCountForParty) {
            totalSeatsByPartyId.set(partyIdKey, {
                politicalPartyId: allocation.politicalPartyId,
                politicalPartyName: allocation.politicalPartyName,
                totalCouncilSeatCountForParty: allocation.totalCouncilSeatCountForParty,
            });
        }
    }

    // Håll koll på vilka partier som redan ingår i en grupp, för att bygga resten (ogrupperade) efteråt
    const groupedPartyIds = new Set();

    const groupUnits = partyGroups.map((group, index) => {
        const memberIds = Array.from(group.partyIds);
        let totalSeats = 0;
        const partyNames = [];

        for (const partyIdKey of memberIds) {
            groupedPartyIds.add(String(partyIdKey));
            const party = totalSeatsByPartyId.get(String(partyIdKey));
            if (party) {
                totalSeats += party.totalCouncilSeatCountForParty;
                partyNames.push(party.politicalPartyName);
            }
        }

        return {
            id: `group-${index}-${group.name}`,
            name: group.name,
            partyNames,
            totalCouncilSeatCountForParty: totalSeats,
        };
    });

    // Ogrupperade partier blir egna, ensamma enheter
    const ungroupedUnits = Array.from(totalSeatsByPartyId.entries())
        .filter(([partyIdKey]) => !groupedPartyIds.has(partyIdKey))
        .map(([, party]) => ({
            id: `party-${party.politicalPartyId}`,
            name: party.politicalPartyName,
            partyNames: [party.politicalPartyName],
            totalCouncilSeatCountForParty: party.totalCouncilSeatCountForParty,
        }));

    return [...groupUnits, ...ungroupedUnits].filter(unit => unit.totalCouncilSeatCountForParty > 0);
}

// JS-portering av BoardSeatAllocationCalculator.CalculateBoardSeatAllocations, fast per enhet (grupp eller ensamt parti).
// Returnerar även en fullständig jämförelsetalsmatris (alla enheter, alla steg) och vilken enhet som vann varje steg,
// eftersom "results" (i linje med backend) bara innehåller raderna för vinnaren/lottningsdeltagarna per steg.
// eslint-disable-next-line react-refresh/only-export-components
export function createTieBreakers(councilAllocations, partyGroups, maxBoardSeatCount) {
    const units = buildUnits(councilAllocations ?? [], partyGroups ?? []);
    const { winnerByStep } = calculateBoardSeatAllocations(units, maxBoardSeatCount);

    return Object.fromEntries(winnerByStep);
}

function calculateBoardSeatAllocations(units, maxBoardSeatCount, tieBreakers = {}) {
    if (!units || units.length === 0 || maxBoardSeatCount <= 0) {
        return { results: [], comparisonMatrix: new Map(), winnerByStep: new Map() };
    }

    const divisor = new Map(units.map(unit => [unit.id, 1]));
    const results = [];
    const comparisonMatrix = new Map(); // step -> Map(unitId -> comparisonNumber)
    const winnerByStep = new Map();     // step -> unitId (vid lottning: den som faktiskt vann)

    for (let seat = 1; seat <= maxBoardSeatCount; seat++) {
        const comparisonList = units.map(unit => ({
            unitId: unit.id,
            comparisonNumber: unit.totalCouncilSeatCountForParty / divisor.get(unit.id),
        }));

        // Spara ALLA enheters jämförelsetal för det här steget, inte bara vinnarens
        comparisonMatrix.set(seat, new Map(comparisonList.map(x => [x.unitId, x.comparisonNumber])));

        const maxComparisonNumber = Math.max(...comparisonList.map(x => x.comparisonNumber));
        const topUnits = comparisonList.filter(x => x.comparisonNumber === maxComparisonNumber);

        // Om flera enheter har samma jämförelsetal avgör lottning vem som får mandatet
        
        
        const savedWinnerId = tieBreakers[seat];
        const winner = topUnits.length > 1
            ? topUnits.find(unit => unit.unitId === savedWinnerId)
                ?? topUnits[Math.floor(Math.random() * topUnits.length)]
            : topUnits[0];

        winnerByStep.set(seat, winner.unitId);

        for (const unit of topUnits) {
            const isWinner = winner.unitId === unit.unitId;

            results.push({
                unitId: unit.unitId,
                allocationDivisor: divisor.get(unit.unitId),
                comparisonNumber: unit.comparisonNumber,
                seatAllocationStep: seat,
                wonSeat: isWinner,
                wonByLotDrawing: isWinner && topUnits.length > 1,
                lotDrawingGroupId: topUnits.length > 1 ? seat : 0,
            });

            if (isWinner) {
                divisor.set(unit.unitId, divisor.get(unit.unitId) + 1);
            }
        }
    }

    return { results, comparisonMatrix, winnerByStep };
}

function createCumulativeSeatCountPerStep(allocations, maxBoardSeats) {
    const wonSeats = allocations
        .filter(allocation => allocation.wonSeat)
        .sort((a, b) => a.seatAllocationStep - b.seatAllocationStep);

    const cumulativeSeatsPerStep = new Map();
    const totalSeatsPerUnitSoFar = new Map();

    let seatIndex = 0;
    for (let step = 1; step <= maxBoardSeats; step++) {
        while (seatIndex < wonSeats.length && wonSeats[seatIndex].seatAllocationStep === step) {
            const allocatedSeat = wonSeats[seatIndex];
            totalSeatsPerUnitSoFar.set(allocatedSeat.unitId, (totalSeatsPerUnitSoFar.get(allocatedSeat.unitId) ?? 0) + 1);
            seatIndex++;
        }
        cumulativeSeatsPerStep.set(step, new Map(totalSeatsPerUnitSoFar));
    }
    return cumulativeSeatsPerStep;
}

function getOddBoardSeats(maxBoardSeats) {
    const steps = [];
    if (maxBoardSeats >= 5) {
        for (let seat = 5; seat <= maxBoardSeats; seat += 2) {
            steps.push(seat);
        }
    }
    return steps;
}

export default PartyGroupsBoardAllocationsTable;
