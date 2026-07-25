using Microsoft.EntityFrameworkCore;
using Repositories.Entities;

namespace Repositories.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<CouncilSeatAllocation> CouncilSeatAllocations { get; set; }
        public DbSet<CouncilSeatAllocationScenario> CouncilSeatAllocationsScenarios { get; set; }
        public DbSet<ElectionResult> ElectionResults { get; set; }
        public DbSet<Election> Elections { get; set; }
        public DbSet<Municipality> Municipalities { get; set; }
        public DbSet<PoliticalParty> PoliticalParties { get; set; }
        public DbSet<User> Users { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
        }
    }
}
