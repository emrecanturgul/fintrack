using FinTrack.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FinTrack.Infrastructure.Persistence.Configurations;

internal sealed class TransactionConfiguration : IEntityTypeConfiguration<Transaction>
{
    public void Configure(EntityTypeBuilder<Transaction> builder)
    {
        builder.Property(t => t.Description).HasMaxLength(500);
        builder.Property(t => t.Currency).HasMaxLength(3).IsFixedLength();
        builder.Property(t => t.FxRate).HasPrecision(18, 8);
        builder.Property(t => t.Merchant).HasMaxLength(200);
        builder.Property(t => t.Note).HasMaxLength(1000);
        builder.Property(t => t.DedupHash).HasMaxLength(64);

        builder.HasOne<User>().WithMany().HasForeignKey(t => t.UserId).OnDelete(DeleteBehavior.Cascade);
        builder.HasOne<Account>().WithMany().HasForeignKey(t => t.AccountId).OnDelete(DeleteBehavior.Cascade);
        builder.HasOne<Category>().WithMany().HasForeignKey(t => t.CategoryId).OnDelete(DeleteBehavior.SetNull);
        builder.HasOne<ImportJob>().WithMany().HasForeignKey(t => t.ImportJobId).OnDelete(DeleteBehavior.SetNull);

        builder.HasIndex(t => new { t.AccountId, t.DedupHash }).IsUnique();
        builder.HasIndex(t => new { t.UserId, t.Date });
    }
}
