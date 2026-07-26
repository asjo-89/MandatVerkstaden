using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Repositories.Entities;
using System.Text.Json;

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

            var file = Path.Combine(AppContext.BaseDirectory, "SeedData", "Municipalities.txt");
            var json = File.ReadAllText(file);

            var municipalities = JsonSerializer.Deserialize<List<Municipality>>(
                json,
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                })!;

            builder.HasData(
                municipalities.Select((m, index) => new Municipality
                {
                    Id = index + 1,
                    Name = m.Name
                }));
        }
    }
}
