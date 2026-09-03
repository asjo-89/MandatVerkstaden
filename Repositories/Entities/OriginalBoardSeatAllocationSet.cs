namespace Repositories.Entities;

public class OriginalBoardSeatAllocationSet
{
    public int Id { get; set; }
    public int MaxSeatCount { get; set; }


    #region Navigation properties
    public int OriginalElectionResultSetId { get; set; }
    public OriginalElectionResultSet OriginalElectionResultSet { get; set; } = null!;

    public ICollection<OriginalBoardSeatAllocation> OriginalBoardSeatAllocations { get; set; } = [];
    #endregion
}
