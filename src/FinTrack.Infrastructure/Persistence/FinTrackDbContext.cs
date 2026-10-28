using FinTrack.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace FinTrack.Infrastructure.Persistence;

public class FinTrackDbContext(DbContextOptions<FinTrackDbContext> options) : DbContext(options)
{
    public DbSet<User> Users => Set<User>();
    public DbSet<RefreshToken> RefreshTokens => Set<RefreshToken>();
    public DbSet<ApiKey> ApiKeys => Set<ApiKey>();
    public DbSet<Account> Accounts => Set<Account>();
    public DbSet<ImportJob> ImportJobs => Set<ImportJob>();
    public DbSet<Transaction> Transactions => Set<Transaction>();
    public DbSet<Category> Categories => Set<Category>();
    public DbSet<CategoryRule> CategoryRules => Set<CategoryRule>();
    public DbSet<Budget> Budgets => Set<Budget>();
    public DbSet<Subscription> Subscriptions => Set<Subscription>();
    public DbSet<FxRate> FxRates => Set<FxRate>();
    public DbSet<MonthlyInsight> MonthlyInsights => Set<MonthlyInsight>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(FinTrackDbContext).Assembly);
    }

    protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
    {
        configurationBuilder.Properties<decimal>().HavePrecision(19, 4);
        configurationBuilder.Properties<Enum>().HaveConversion<string>().HaveMaxLength(32);
    }
}
