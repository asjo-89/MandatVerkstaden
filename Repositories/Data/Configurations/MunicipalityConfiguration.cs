using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Repositories.Entities;

namespace Repositories.Data.Configurations
{
    public class MunicipalityConfiguration : IEntityTypeConfiguration<Municipality>
    {
        public void Configure(EntityTypeBuilder<Municipality> builder)
        {
            #region Navigation
            builder.HasMany(m => m.PoliticalParties)
                .WithMany(pp => pp.Municipalities)
                .UsingEntity(join => join.ToTable("MunicipalityPoliticalParty"));
            #endregion


            builder.HasIndex(m => m.Name)
                .IsUnique();

            builder.Property(m => m.Name)
                .HasMaxLength(100)
                .IsRequired();
        }
    }
}
