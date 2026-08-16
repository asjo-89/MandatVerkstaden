using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Repositories.Entities;

namespace Repositories.Data.Configurations;

public class ScenarioConstituencyVoteResultConfiguration : IEntityTypeConfiguration<ScenarioConstituencyVoteResult>
{
    public void Configure(EntityTypeBuilder<ScenarioConstituencyVoteResult> builder)
    {
        builder
            .HasIndex(scvr => new { scvr.PoliticalPartyId, scvr.ScenarioId })
            .IsUnique();

        builder
            .HasOne(scvr => scvr.PoliticalParty)
            .WithMany(pp => pp.ScenarioConstituencyVoteResults)
            .HasForeignKey(scvr => scvr.PoliticalPartyId)
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasOne(scvr => scvr.Scenario)
            .WithMany(s => s.ScenarioConstituencyVoteResults)
            .HasForeignKey(scvr => scvr.ScenarioId)
            .OnDelete(DeleteBehavior.Cascade);

        builder
            .HasOne(scvr => scvr.ElectionConstituency)
            .WithMany(ec => ec.ScenarioConstituencyVoteResults)
            .HasForeignKey(scvr => scvr.ElectionConstituencyId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
