using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Repositories.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Repositories.Data.Configurations
{
    public class ElectionResultsConfiguration : IEntityTypeConfiguration<ElectionResult>
    {
        public void Configure(EntityTypeBuilder<ElectionResult> builder)
        {
            #region Navigation
            builder.HasOne(er => er.Municipality)
                .WithMany()
                .HasForeignKey(er => er.MunicipalityId);

            builder.HasOne(er => er.PoliticalParty)
                .WithMany()
                .HasForeignKey(er => er.PoliticalPartyId);

            builder.HasOne(er => er.Election)
                .WithMany()
                .HasForeignKey(er => er.ElectionId);
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
