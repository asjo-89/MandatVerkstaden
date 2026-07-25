namespace Repositories.Entities
{
    public class Municipality
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public int CouncilSeatCount { get; set; }


        #region Navigation properties
        public ICollection<CouncilSeatAllocation> CouncilSeatAllocations { get; init; } = [];
        public ICollection<CouncilSeatAllocationScenario> CouncilSeatAllocationScenarios { get; init; } = [];
        public ICollection<ElectionResult> ElectionResults { get; init; } = [];
        public ICollection<PoliticalParty> PoliticalParties { get; init; } = [];
        #endregion
    }
}
