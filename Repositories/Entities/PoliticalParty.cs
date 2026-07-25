namespace Repositories.Entities
{
    public class PoliticalParty
    {
        public int Id { get; set; }
        public string PartyName { get; set; } = null!;
        public bool IsLocal { get; set; }


        #region Navigation properties
        public ICollection<ElectionResult> ElectionResults { get; init; } = [];
        public ICollection<CouncilSeatAllocationScenario> CouncilSeatAllocationScenarios { get; init; } = [];
        public ICollection<Municipality> Municipalities { get; init; } = [];
        #endregion
    }
}
