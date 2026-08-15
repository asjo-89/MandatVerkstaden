using Microsoft.EntityFrameworkCore;
using Repositories.Entities;

namespace Repositories.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Election> Elections { get; set; }
        public DbSet<ElectionConstituency> ElectionConstituencies { get; set; }
        public DbSet<Municipality> Municipalities { get; set; }
        public DbSet<OriginalConstituencyVoteResult> OriginalConstituencyVoteResults { get; set; }
        public DbSet<OriginalCouncilSeatAllocation> OriginalCouncilSeatAllocations { get; set; }
        public DbSet<OriginalElectionResultSet> OriginalElectionResultSets { get; set; }
        public DbSet<PartyGroup> PartyGroups { get; set; }
        public DbSet<PoliticalParty> PoliticalParties { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }
        public DbSet<Scenario> Scenarios { get; set; }
        public DbSet<ScenarioConstituencyVoteResult> ScenarioConstituencyVoteResults { get; set; }
        public DbSet<ScenarioCouncilSeatAllocation> ScenarioCouncilSeatAllocations { get; set; }
        public DbSet<User> Users { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
        }
    }
}
