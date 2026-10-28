using FinTrack.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FinTrack.Infrastructure.Persistence.Configurations;

internal sealed class FxRateConfiguration : IEntityTypeConfiguration<FxRate>
{
    public void Configure(EntityTypeBuilder<FxRate> builder)
    {
        builder.Property(r => r.Base).HasMaxLength(3).IsFixedLength();
        builder.Property(r => r.Quote).HasMaxLength(3).IsFixedLength();
        builder.Property(r => r.Rate).HasPrecision(18, 8);
        builder.Property(r => r.Source).HasMaxLength(50);

        builder.HasIndex(r => new { r.Base, r.Quote, r.Date }).IsUnique();
    }
}
