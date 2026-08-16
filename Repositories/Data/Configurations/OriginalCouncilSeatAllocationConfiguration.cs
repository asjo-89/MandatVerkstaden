using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Repositories.Entities;

namespace Repositories.Data.Configurations;

public class OriginalCouncilSeatAllocationConfiguration : IEntityTypeConfiguration<OriginalCouncilSeatAllocation>
{
    public void Configure(EntityTypeBuilder<OriginalCouncilSeatAllocation> builder)
    {
        builder
            .HasIndex(ocsa => new { ocsa.AllocatedSeat, ocsa.PoliticalPartyId, ocsa.OriginalElectionResultSetId })
            .IsUnique();

        builder
            .Property(ocsa => ocsa.AllocationDivisor)
            .HasPrecision(18, 4);

        builder
            .Property(ocsa => ocsa.ComparisonNumber)
            .HasPrecision(18, 4);


        builder
            .HasOne(ocsa => ocsa.PoliticalParty)
            .WithMany(pp => pp.OriginalCouncilSeatAllocations)
            .HasForeignKey(ocsa => ocsa.PoliticalPartyId)
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasOne(ocsa => ocsa.OriginalElectionResultSet)
            .WithMany(oers => oers.OriginalCouncilSeatAllocations)
            .HasForeignKey(oers => oers.OriginalElectionResultSetId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
