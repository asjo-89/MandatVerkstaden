using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Repositories.Entities;

namespace Repositories.Data.Configurations;

public class OriginalElectionResultSetConfiguration : IEntityTypeConfiguration<OriginalElectionResultSet>
{
    public void Configure(EntityTypeBuilder<OriginalElectionResultSet> builder)
    {
        builder
            .HasIndex(oers => new { oers.UserId, oers.MunicipalityId, oers.ElectionId })
            .IsUnique();

        builder
            .HasOne(oers => oers.Election)
            .WithMany(e => e.OriginalElectionResultSets)
            .HasForeignKey(oers => oers.ElectionId)
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasOne(oers => oers.Municipality)
            .WithMany(m => m.OriginalElectionResultSets)
            .HasForeignKey(oers => oers.MunicipalityId)
            .OnDelete(DeleteBehavior.Cascade);

        builder
            .HasOne(oers => oers.User)
            .WithMany(u => u.OriginalElectionResultSets)
            .HasForeignKey(oers => oers.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
