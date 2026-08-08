namespace Repositories.Entities
{
    public class Election
    {
        public int Id { get; set; }
        public int ElectionYear { get; set; }


        #region Navigation properties
        public ICollection<CouncilSeatAllocation> CouncilSeatAllocations { get; init; } = [];
        public ICollection<CouncilSeatAllocationScenario> CouncilSeatAllocationScenarios { get; init; } = [];
        public ICollection<ElectionResult> ElectionResults { get; init; } = [];
        public ICollection<ElectionDistrict> ElectionDistricts { get; set; } = [];
        #endregion
    }
}
