using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Repositories.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Repositories.Data.Configurations;

public class PoliticalPartyConfiguration : IEntityTypeConfiguration<PoliticalParty>
{
    public void Configure(EntityTypeBuilder<PoliticalParty> builder)
    {
        builder
            .HasIndex(pp => new { pp.Name, pp.MunicipalityId })
            .IsUnique();

        builder
            .Property(pp => pp.Name)
            .HasMaxLength(200)
            .IsRequired();

        builder
            .Property(pp => pp.UserId)
            .IsRequired(false);

        builder
            .Property(pp => pp.MunicipalityId)
            .IsRequired(false);

        builder
            .HasOne(pp => pp.User)
            .WithMany(u => u.PoliticalParties)
            .HasForeignKey(pp => pp.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder
            .HasOne(pp => pp.Municipality)
            .WithMany(m => m.PoliticalParties)
            .HasForeignKey(pp => pp.MunicipalityId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
