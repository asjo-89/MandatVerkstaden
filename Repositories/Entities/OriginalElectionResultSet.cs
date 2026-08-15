namespace Repositories.Entities;

public class OriginalElectionResultSet
{
    public int Id { get; set; }
    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

    #region Navigation properties
    public int ElectionId { get; set; }
    public Election Election { get; set; } = null!;

    public int MunicipalityId { get; set; }
    public Municipality Municipality { get; set; } = null!;

    public int UserId { get; set; }
    public User User { get; set; } = null!;
    #endregion
}