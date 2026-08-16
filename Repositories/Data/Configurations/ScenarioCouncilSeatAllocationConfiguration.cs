using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Repositories.Entities;

namespace Repositories.Data.Configurations;

public class ScenarioCouncilSeatAllocationConfiguration : IEntityTypeConfiguration<ScenarioCouncilSeatAllocation>
{
    public void Configure(EntityTypeBuilder<ScenarioCouncilSeatAllocation> builder)
    {
        builder
            .HasIndex(scsa => new { scsa.AllocatedSeat, scsa.ScenarioId, scsa.PoliticalPartyId })
            .IsUnique();

        builder
            .Property(scsa => scsa.AllocationDivisor)
            .HasPrecision(18, 4);

        builder
            .Property(scsa => scsa.ComparisonNumber)
            .HasPrecision(18, 4);

        builder
            .HasOne(scsa => scsa.Scenario)
            .WithMany(s => s.ScenarioCouncilSeatAllocations)
            .HasForeignKey(scsa => scsa.ScenarioId)
            .OnDelete(DeleteBehavior.Cascade);

        builder
            .HasOne(scsa => scsa.PoliticalParty)
            .WithMany(scsa => scsa.ScenarioCouncilSeatAllocations)
            .HasForeignKey(scsa => scsa.PoliticalPartyId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
    