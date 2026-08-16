using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Repositories.Entities;

namespace Repositories.Data.Configurations;

public class ElectionConfiguration : IEntityTypeConfiguration<Election>
{
    public void Configure(EntityTypeBuilder<Election> builder)
    {
        builder.HasIndex(e => e.ElectionYear)
            .IsUnique();

        builder.Property(e => e.ElectionYear)
            .HasMaxLength(4);                
    }
}
