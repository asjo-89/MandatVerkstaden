using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Repositories.Entities;

namespace Repositories.Data.Configurations;

public class ScenarioConfiguration : IEntityTypeConfiguration<Scenario>
{
    public void Configure(EntityTypeBuilder<Scenario> builder)
    {
        builder
            .HasIndex(s => new { s.Name, s.UserId })
            .IsUnique();

        builder
            .Property(s => s.Name)
            .HasMaxLength(200)
            .IsRequired();

        builder
            .HasOne(s => s.User)
            .WithMany(u => u.Scenarios)
            .HasForeignKey(s => s.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasOne(s => s.OriginalElectionResultSet)
            .WithMany(oers => oers.Scenarios)
            .HasForeignKey(s => s.OriginalElectionResultSetId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
