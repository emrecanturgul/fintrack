using FinTrack.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FinTrack.Infrastructure.Persistence.Configurations;

internal sealed class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.Property(u => u.Email).HasMaxLength(256);
        builder.Property(u => u.PasswordHash).HasMaxLength(256);
        builder.Property(u => u.DisplayName).HasMaxLength(100);
        builder.Property(u => u.BaseCurrency).HasMaxLength(3).IsFixedLength();

        builder.HasIndex(u => u.Email).IsUnique();
    }
}
