namespace Repositories.Entities;

public class PartyGroup
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;




    #region Navigation properties
    public Guid UserId { get; set; }
    public User User { get; set; } = null!;

    public ICollection<Scenario> Scenarios { get; set; } = [];
    public ICollection<PoliticalParty> PoliticalParties { get; set; } = [];
    #endregion
}