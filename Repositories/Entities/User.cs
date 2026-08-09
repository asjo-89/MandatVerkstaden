namespace Repositories.Entities
{
    public class User
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string UserName { get; set; } = default!;
        public string Email { get; set; } = null!;
        public string PasswordHash { get; set; } = default!;
        public string Role { get; set; } = "User";
        public int FailedLogIns { get; set; } = default!;
        public bool IsLockedOut { get; set; } = false;


        #region Navigation properties
        public ICollection<RefreshToken> RefreshTokens { get; init; } = [];
        #endregion
    }
}
