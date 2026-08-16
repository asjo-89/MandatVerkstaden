using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Repositories.Entities;

namespace Repositories.Data.Configurations;

public class PartyGroupConfiguration : IEntityTypeConfiguration<PartyGroup>
{
    public void Configure(EntityTypeBuilder<PartyGroup> builder)
    {
        builder
            .HasIndex(pg => new { pg.UserId, pg.Name })
            .IsUnique();

        builder
            .Property(pg => pg.Name)
            .HasMaxLength(100)
            .IsRequired();

        builder
            .HasOne(pg => pg.User)
            .WithMany(u => u.PartyGroups)
            .HasForeignKey(pg => pg.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder
            .HasMany(pg => pg.PoliticalParties)
            .WithMany(pp => pp.PartyGroups)
            .UsingEntity<Dictionary<string, object>>(
                "PartyGroupPoliticalParty",
                join => join
                    .HasOne<PoliticalParty>()
                    .WithMany()
                    .HasForeignKey("PoliticalPartiesId")
                    .OnDelete(DeleteBehavior.ClientCascade),
                join => join
                    .HasOne<PartyGroup>()
                    .WithMany()
                    .HasForeignKey("PartyGroupsId")
                    .OnDelete(DeleteBehavior.Cascade)
            );

        builder
            .HasMany(pg => pg.Scenarios)
            .WithMany(s => s.PartyGroups)
            .UsingEntity<Dictionary<string, object>>(
                "PartyGroupScenario",
                join => join
                    .HasOne<Scenario>()
                    .WithMany()
                    .HasForeignKey("ScenariosId")
                    .OnDelete(DeleteBehavior.ClientCascade),
                join => join
                    .HasOne<PartyGroup>()
                    .WithMany()
                    .HasForeignKey("PartyGroupsId")
                    .OnDelete(DeleteBehavior.Cascade)
            );
    }
}
