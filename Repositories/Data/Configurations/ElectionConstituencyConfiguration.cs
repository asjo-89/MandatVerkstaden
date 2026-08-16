using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Repositories.Entities;

namespace Repositories.Data.Configurations;

public class ElectionConstituencyConfiguration : IEntityTypeConfiguration<ElectionConstituency>
{
    public void Configure(EntityTypeBuilder<ElectionConstituency> builder)
    {
        builder
            .HasIndex(ec => new { ec.Name, ec.ElectionId, ec.MunicipalityId })
            .IsUnique();

        builder
            .Property(ec => ec.Name)
            .HasMaxLength(200)
            .IsRequired();

        builder
            .HasOne(ec => ec.Election)
            .WithMany(e => e.ElectionConstituencies)
            .HasForeignKey(ec => ec.ElectionId)
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasOne(ec => ec.Municipality)
            .WithMany(m => m.ElectionConstituencies)
            .HasForeignKey(ec => ec.MunicipalityId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
