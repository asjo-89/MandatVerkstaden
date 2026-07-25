using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Repositories.Entities;

namespace Repositories.Data.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasIndex(u => u.Email)
               .IsUnique();

            builder.HasIndex(u => u.UserName)
                .IsUnique();

            builder.Property(u => u.Email)
                .HasMaxLength(260)
                .IsRequired();

            builder.Property(u => u.FirstName)
                .HasMaxLength(50)
                .IsRequired();

            builder.Property(u => u.LastName)
                .HasMaxLength(50)
                .IsRequired();

            builder.Property(u => u.HashedPassword)
                .HasMaxLength(200)
                .IsRequired();

            builder.Property(u => u.UserName)
                .HasMaxLength(20)
                .IsRequired();
        }
    }
}
