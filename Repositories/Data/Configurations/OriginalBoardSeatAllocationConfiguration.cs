using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Repositories.Entities;

namespace Repositories.Data.Configurations;

public class OriginalBoardSeatAllocationConfiguration : IEntityTypeConfiguration<OriginalBoardSeatAllocation>
{
    public void Configure(EntityTypeBuilder<OriginalBoardSeatAllocation> builder)
    {
        builder
            .Property(obsa => obsa.AllocationDivisor)
            .HasPrecision(18, 2);

        builder
            .Property(obsa => obsa.ComparisonNumber)
            .HasPrecision(18, 4);

        builder
            .HasIndex(obsa => new
            {
                obsa.OriginalBoardSeatAllocationSetId,
                obsa.SeatAllocationStep,
                obsa.PoliticalPartyId
            })
            .IsUnique();

        builder
            .HasOne(obsa => obsa.OriginalBoardSeatAllocationSet)
            .WithMany(obsas => obsas.OriginalBoardSeatAllocations)
            .HasForeignKey(obsa => obsa.OriginalBoardSeatAllocationSetId)
            .OnDelete(DeleteBehavior.Cascade);

        builder
            .HasOne(obsa => obsa.PoliticalParty)
            .WithMany(pp => pp.OriginalBoardSeatAllocations)
            .HasForeignKey(obsa => obsa.PoliticalPartyId)
            .OnDelete(DeleteBehavior.Restrict);        
    }
}
