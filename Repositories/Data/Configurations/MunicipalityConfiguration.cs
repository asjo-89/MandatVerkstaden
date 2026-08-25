using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Repositories.Entities;

namespace Repositories.Data.Configurations;

public class MunicipalityConfiguration : IEntityTypeConfiguration<Municipality>
{
    public void Configure(EntityTypeBuilder<Municipality> builder)
    {
        builder
            .HasIndex(m => new { m.MunicipalityCode, m.ElectionAreaName })
            .IsUnique();

        builder
            .Property(m => m.MunicipalityCode)
            .IsRequired();

        builder
            .Property(m => m.ElectionAreaName)
            .HasMaxLength(200)
            .IsRequired();
    }
}
