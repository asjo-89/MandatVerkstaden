using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Repositories.Entities;

namespace Repositories.Data.Configurations
{
    public class CouncilSeatAllocationsConfiguration : IEntityTypeConfiguration<CouncilSeatAllocations>
    {
        public void Configure(EntityTypeBuilder<CouncilSeatAllocations> builder)
        {
            #region Navigation
            builder.HasOne(csa => csa.PoliticalParty)
                .WithMany()
                .HasForeignKey(csa => csa.PoliticalPartyId);

            builder.HasOne(csa => csa.Municipality)
                .WithMany()
                .HasForeignKey(csa => csa.MunicipalityId);

            builder.HasOne(csa => csa.Election)
                .WithMany()
                .HasForeignKey(csa => csa.ElectionId);
            #endregion

            builder.HasIndex(csa => new
            {
                csa.MunicipalityId,
                csa.ElectionId,
                csa.SeatNumber
            })
                .IsUnique();

            builder.Property(csa => csa.AllocationDivisor)
                .HasPrecision(7, 2);

            builder.Property(csa => csa.Quotient)
                .HasPrecision(7, 2);
        }
    }
}
