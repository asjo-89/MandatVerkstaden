namespace Repositories.Entities
{
    public class CouncilSeatAllocationScenario
    {
        public int Id { get; set; }
        public string ScenarioName { get; set; } = null!;
        public DateTime CreatedAt { get; set; }

        public int SeatNumber { get; set; }         // Used to create unique index.
        public int PartySeatCountBeforeAllocation { get; set; }
        public decimal ComparisonNumber { get; set; }
        public decimal AllocationDivisor { get; set; }
        public int VoteCountUsed { get; set; }



        #region Navigation properties
        public Guid UserId { get; set; }            // Used to create unique index.
        public User User { get; set; } = null!;

        public int MunicipalityId { get; set; }         // Used to create unique index.
        public Municipality Municipality { get; set; } = null!;

        public int ElectionId { get; set; }         //Used to create unique index.
        public Election Election { get; set; } = null!;

        public int PoliticalPartyId { get; set; }
        public PoliticalParty PoliticalParty { get; set; } = null!;
        #endregion
    }
}
