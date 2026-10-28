using FinTrack.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FinTrack.Infrastructure.Persistence.Configurations;

internal sealed class BudgetConfiguration : IEntityTypeConfiguration<Budget>
{
    public void Configure(EntityTypeBuilder<Budget> builder)
    {
        builder.Property(b => b.Currency).HasMaxLength(3).IsFixedLength();

        builder.HasOne<User>().WithMany().HasForeignKey(b => b.UserId).OnDelete(DeleteBehavior.Cascade);
        builder.HasOne<Category>().WithMany().HasForeignKey(b => b.CategoryId).OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(b => new { b.UserId, b.CategoryId }).IsUnique();
    }
}
