using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Repositories.Entities;

namespace Repositories.Data.Configurations
{
    public class CouncilSeatAllocationConfiguration : IEntityTypeConfiguration<CouncilSeatAllocation>
    {
        public void Configure(EntityTypeBuilder<CouncilSeatAllocation> builder)
        {
            #region Navigation
            builder.HasOne(csa => csa.ElectionResult)
                .WithMany(er => er.CouncilSeatAllocations)
                .HasForeignKey(csa => csa.ElectionResultId)
                .HasPrincipalKey(er => er.Id)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(csa => csa.Municipality)
                .WithMany(m => m.CouncilSeatAllocations)
                .HasForeignKey(csa => csa.MunicipalityId)
                .HasPrincipalKey(m => m.Id)
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(csa => csa.Election)
                .WithMany(e => e.CouncilSeatAllocations)
                .HasForeignKey(csa => csa.ElectionId)
                .HasPrincipalKey(e => e.Id)
                .OnDelete(DeleteBehavior.NoAction);
            #endregion

            builder.HasIndex(csa => new
            {
                csa.MunicipalityId,
                csa.ElectionId,
                csa.SeatNumber
            })
                .IsUnique();

            builder.Property(csa => csa.AllocationDivisor)
                .HasPrecision(18, 4);

            builder.Property(csa => csa.ComparisonNumber)
                .HasPrecision(18, 4);
        }
    }
}
