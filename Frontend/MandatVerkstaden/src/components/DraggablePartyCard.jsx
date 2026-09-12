import {useDraggable} from '@dnd-kit/react';
import PartyCard from './cards/PartyCard';

export function DraggablePartyCard({ partyName, partyId, totalSeats }) {
  const {ref} = useDraggable({
    id: `partyId-${partyId}`,
  });

  return (
    
        <PartyCard
            partyName={partyName}
            totalSeats={totalSeats}
            ref={ref}
            />
  );
}


