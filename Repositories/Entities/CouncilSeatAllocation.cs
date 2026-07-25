namespace Repositories.Entities
{
    public class CouncilSeatAllocation
    {
        public int Id { get; set; } 
        public int SeatNumber { get; set; }         // Used to create unique index
        public int PartySeatCountBeforeAllocation { get; set; }
        public decimal ComparisonNumber { get; set; }
        public decimal AllocationDivisor { get; set; }


        #region Navigation properties
        public int MunicipalityId { get; set; }         // Used to create unique index
        public Municipality Municipality { get; set; } = null!;

        public int ElectionId { get; set; }         // Used to create unique index
        public Election Election { get; set; } = null!;

        public int ElectionResultId { get; set; }
        public ElectionResult ElectionResult { get; set; } = null!;
        #endregion
    }
}
