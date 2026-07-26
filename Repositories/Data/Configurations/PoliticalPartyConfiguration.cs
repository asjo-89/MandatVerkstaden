using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Repositories.Entities;
using System.Text.Json;

namespace Repositories.Data.Configurations
{
    public class PoliticalPartyConfiguration : IEntityTypeConfiguration<PoliticalParty>
    {
        public void Configure(EntityTypeBuilder<PoliticalParty> builder)
        {
            builder.HasIndex(pp => pp.PartyName)
                .IsUnique();

            builder.Property(pp => pp.PartyName)
                .HasMaxLength(200)
                .IsRequired();

            var file = Path.Combine(AppContext.BaseDirectory, "SeedData", "PoliticalParties.txt");
            var json = File.ReadAllText(file);

            var politicalParties = JsonSerializer.Deserialize<List<PoliticalParty>>(
                json,
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                })!;

            builder.HasData(
                politicalParties.Select((m, index) => new PoliticalParty
                {
                    Id = index + 1,
                    PartyName = m.PartyName
                }));
        }
    }
}
