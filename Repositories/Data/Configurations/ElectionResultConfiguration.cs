using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Repositories.Entities;

namespace Repositories.Data.Configurations
{
    public class ElectionResultConfiguration : IEntityTypeConfiguration<ElectionResult>
    {
        public void Configure(EntityTypeBuilder<ElectionResult> builder)
        {
            #region Navigation
            builder.HasOne(er => er.Municipality)
                .WithMany(m => m.ElectionResults)
                .HasForeignKey(er => er.MunicipalityId)
                .HasPrincipalKey(m => m.Id)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(er => er.PoliticalParty)
                .WithMany(pp => pp.ElectionResults)
                .HasForeignKey(er => er.PoliticalPartyId)
                .HasPrincipalKey(pp => pp.Id)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(er => er.Election)
                .WithMany(e => e.ElectionResults)
                .HasForeignKey(er => er.ElectionId)
                .HasPrincipalKey(e => e.Id)
                .OnDelete(DeleteBehavior.Cascade);
            #endregion

            builder.HasIndex(er => new
            {
                er.MunicipalityId,
                er.PoliticalPartyId,
                er.ElectionId
            })
                .IsUnique();

            builder.Property(er => er.VotePercentage)
                .HasPrecision(7, 2);
        }
    }
}
