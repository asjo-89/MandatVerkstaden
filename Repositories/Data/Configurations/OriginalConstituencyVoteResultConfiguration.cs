using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Repositories.Entities;

namespace Repositories.Data.Configurations;

public class OriginalConstituencyVoteResultConfiguration : IEntityTypeConfiguration<OriginalConstituencyVoteResult>
{
    public void Configure(EntityTypeBuilder<OriginalConstituencyVoteResult> builder)
    {
        builder
            .HasIndex(ocvr => new { ocvr.OriginalElectionResultSetId, ocvr.ElectionConstituencyId, ocvr.PoliticalPartyId })
            .IsUnique();

        builder
            .HasOne(ocvr => ocvr.OriginalElectionResultSet)
            .WithMany(oers => oers.OriginalConstituencyVoteResults)
            .HasForeignKey(ocvr => ocvr.OriginalElectionResultSetId)
            .OnDelete(DeleteBehavior.Cascade);

        builder
            .HasOne(ocvr => ocvr.ElectionConstituency)
            .WithMany(ec => ec.OriginalConstituencyVoteResults)
            .HasForeignKey(ocvr => ocvr.ElectionConstituencyId)
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasOne(ocvr => ocvr.PoliticalParty)
            .WithMany(pp => pp.OriginalConstituencyVoteResults)
            .HasForeignKey(ocvr => ocvr.PoliticalPartyId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
