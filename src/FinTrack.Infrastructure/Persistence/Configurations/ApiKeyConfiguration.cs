using FinTrack.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FinTrack.Infrastructure.Persistence.Configurations;

internal sealed class ApiKeyConfiguration : IEntityTypeConfiguration<ApiKey>
{
    public void Configure(EntityTypeBuilder<ApiKey> builder)
    {
        builder.Property(k => k.Name).HasMaxLength(100);
        builder.Property(k => k.Prefix).HasMaxLength(16);
        builder.Property(k => k.KeyHash).HasMaxLength(128);

        builder.HasOne<User>().WithMany().HasForeignKey(k => k.UserId).OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(k => k.Prefix).IsUnique();
    }
}
