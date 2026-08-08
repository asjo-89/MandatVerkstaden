namespace Repositories.Entities
{
    public class CouncilSeatAllocation
    {
        public int Id { get; set; } 
        public int SeatNumber { get; set; }         // Used to create unique index
        public int PartySeatCountBeforeAllocation { get; set; }

        // Party with the highest ComparisonNumber gets the seat
        public decimal ComparisonNumber { get; set; }

        //AllocationDivisor is used to get the comparison number. 
        //1st round, total number of votes for party / 1.2 = ComparisonNumber. 
        //2nd round and forward, ComparisonNumber / next number wihtout decimals above 1.2 (eg. 2, 3, 4) = new ComparisonNumber
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
