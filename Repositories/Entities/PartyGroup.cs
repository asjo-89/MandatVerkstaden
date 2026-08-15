namespace Repositories.Entities;

public class PartyGroup
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

    #region Navigation properties
    public int ScenarioId { get; set; }
    public Scenario Scenario { get; set; } = null!;

    public ICollection<PoliticalParty> PoliticalParties { get; set; } = [];
    #endregion
}