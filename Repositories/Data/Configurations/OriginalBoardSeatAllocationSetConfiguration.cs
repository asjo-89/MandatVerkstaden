using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Repositories.Entities;

namespace Repositories.Data.Configurations;

public class OriginalBoardSeatAllocationSetConfiguration : IEntityTypeConfiguration<OriginalBoardSeatAllocationSet>
{
    public void Configure(EntityTypeBuilder<OriginalBoardSeatAllocationSet> builder)
    {
        //builder
        //    .HasIndex(obsas => new { obsas.OriginalElectionResultSetId, obsas.MaxSeatCount })
        //    .IsUnique();

        builder
            .HasOne(obsas => obsas.OriginalElectionResultSet)
            .WithOne(oers => oers.OriginalBoardSeatAllocationSet)
            .HasForeignKey<OriginalBoardSeatAllocationSet>(obsas => obsas.OriginalElectionResultSetId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
