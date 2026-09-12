import {useDroppable} from '@dnd-kit/react';
import { DeleteButton } from './buttons/DeleteButton';

export function DroppablePartyCard({ groupName, groupId, children, onDelete }) {
  const {ref} = useDroppable({
    id: `groupId-${groupId}`,
  });

  return (
    
        <div ref={ref} className="droppable-party-card-container">
          <div className="droppable-party-card-header">
            <h4>{groupName}</h4>
            {groupId !== "unassigned" && (
              <DeleteButton
                className="card-delete-button"
                btnText="X"
                onClick={() => onDelete?.(groupId)}
              />
            )}
          </div>
          <div className="party-groups-cards">
            {children}
          </div>
        </div>
  );
}


