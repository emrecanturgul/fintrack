using FinTrack.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FinTrack.Infrastructure.Persistence.Configurations;

internal sealed class MonthlyInsightConfiguration : IEntityTypeConfiguration<MonthlyInsight>
{
    public void Configure(EntityTypeBuilder<MonthlyInsight> builder)
    {
        builder.Property(i => i.Model).HasMaxLength(100);

        builder.HasOne<User>().WithMany().HasForeignKey(i => i.UserId).OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(i => new { i.UserId, i.Month }).IsUnique();
    }
}
