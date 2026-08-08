namespace Repositories.Entities
{
    public class Municipality
    {
        public int Id { get; set; }
        public int MunicipalityCode { get; set; }
        public string ElectionAreaName { get; set; } = null!;


        #region Navigation properties
        public ICollection<HistoryCouncilSeatCount> HistoryCouncilSeatCounts { get; init; } = [];
        public ICollection<ElectionDistrict> ElectionDistricts { get; set; } = [];
        public ICollection<CouncilSeatAllocation> CouncilSeatAllocations { get; init; } = [];
        public ICollection<CouncilSeatAllocationScenario> CouncilSeatAllocationScenarios { get; init; } = [];
        public ICollection<ElectionResult> ElectionResults { get; init; } = [];
        public ICollection<PoliticalParty> PoliticalParties { get; init; } = [];
        #endregion
    }
}
