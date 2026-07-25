using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Repositories.Entities;

namespace Repositories.Data.Configurations
{
    public class CouncilSeatAllocationScenarioConfiguration : IEntityTypeConfiguration<CouncilSeatAllocationScenario>
    {
        public void Configure(EntityTypeBuilder<CouncilSeatAllocationScenario> builder)
        {
            #region Navigation
            builder.HasOne(csas => csas.User)
                .WithMany(u => u.CouncilSeatAllocationScenarios)
                .HasForeignKey(csas => csas.UserId)
                .HasPrincipalKey(u => u.Id)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(csas => csas.Municipality)
                .WithMany(m => m.CouncilSeatAllocationScenarios)
                .HasForeignKey(csas => csas.MunicipalityId)
                .HasPrincipalKey(m => m.Id)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(csas => csas.Election)
                .WithMany(e => e.CouncilSeatAllocationScenarios)
                .HasForeignKey(csas => csas.ElectionId)
                .HasPrincipalKey(e => e.Id)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(csas => csas.PoliticalParty)
                .WithMany(pp => pp.CouncilSeatAllocationScenarios)
                .HasForeignKey(csas => csas.PoliticalPartyId)
                .HasPrincipalKey(pp => pp.Id)
                .OnDelete(DeleteBehavior.Cascade);
            #endregion

            builder.HasIndex(csas => new
            {
                csas.MunicipalityId,
                csas.ElectionId,
                csas.SeatNumber,
                csas.UserId
            })
                .IsUnique();

            builder.Property(csas => csas.AllocationDivisor)
                .HasPrecision(18, 4);

            builder.Property(csas => csas.ComparisonNumber)
                .HasPrecision(18, 4);

            builder.Property(csas => csas.ScenarioName)
                .HasMaxLength(500)
                .IsRequired();
        }
    }
}
