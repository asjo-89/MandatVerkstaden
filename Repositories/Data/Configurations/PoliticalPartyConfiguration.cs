using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Repositories.Entities;

namespace Repositories.Data.Configurations
{
    public class PoliticalPartyConfiguration : IEntityTypeConfiguration<PoliticalParty>
    {
        public void Configure(EntityTypeBuilder<PoliticalParty> builder)
        {
            builder.HasIndex(pp => pp.PartyName)
                .IsUnique();

            builder.Property(pp => pp.PartyName)
                .IsRequired();

            builder.Property(pp => pp.IsLocal)
                .IsRequired();
        }
    }
}
