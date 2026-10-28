using FinTrack.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FinTrack.Infrastructure.Persistence.Configurations;

internal sealed class ImportJobConfiguration : IEntityTypeConfiguration<ImportJob>
{
    public void Configure(EntityTypeBuilder<ImportJob> builder)
    {
        builder.Property(j => j.FileName).HasMaxLength(255);
        builder.Property(j => j.FileHash).HasMaxLength(64);
        builder.Property(j => j.Format).HasMaxLength(50);

        builder.HasOne<User>().WithMany().HasForeignKey(j => j.UserId).OnDelete(DeleteBehavior.Cascade);
        builder.HasOne<Account>().WithMany().HasForeignKey(j => j.AccountId).OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(j => new { j.UserId, j.FileHash }).IsUnique();
    }
}
