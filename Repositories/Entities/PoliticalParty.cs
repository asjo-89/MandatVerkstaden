namespace Repositories.Entities;

public class PoliticalParty
{
    public int Id { get; set; }
    public required string PartyName { get; set; }
    public bool IsLocal { get; set; } = false;

    #region Navigation properties
    public int UserId { get; set; }
    public User User { get; set; } = null!;

    public int MunicipalityId { get; set; }
    public Municipality Municipality { get; set; } = null!;
    #endregion
}
