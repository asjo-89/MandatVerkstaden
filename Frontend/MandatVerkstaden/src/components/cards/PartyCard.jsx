const PartyCard = ({ partyName, totalSeats, ref }) => {
    
    return (
        <button ref={ref} className="party-card">
            {partyName} - ({totalSeats})
        </button>
    );
};

export default PartyCard;