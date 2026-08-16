namespace Repositories.Entities;

public class User
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public required string UserName { get; set; }
    public required string Email { get; set; }
    public required string PasswordHash { get; set; }
    public string Role { get; set; } = "User";
    public int FailedLogIns { get; set; } 
    public bool IsLockedOut { get; set; } = false;




    #region Navigation properties
    public ICollection<OriginalElectionResultSet> OriginalElectionResultSets { get; init; } = [];
    public ICollection<PartyGroup> PartyGroups { get; set; } = [];
    public ICollection<RefreshToken> RefreshTokens { get; init; } = [];
    public ICollection<PoliticalParty> PoliticalParties { get; set; } = [];
    public ICollection<Scenario> Scenarios { get; set; } = [];
    #endregion
}
