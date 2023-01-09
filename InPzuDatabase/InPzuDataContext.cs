using InvestementsTracker.Comparers;
using InvestementsTracker.Converters;
using Microsoft.EntityFrameworkCore;

namespace InvestementsTracker.InPzuDatabase;
public class InPzuDataContext : DbContext
{
    public InPzuDataContext(DbContextOptions<InPzuDataContext> options) : base(options)
    {
    }

    public DbSet<InPzuPortfolio> InPzuPortfolios { get; set; }
    public DbSet<InPzuPosition> InPzuPositions { get; set; }
    public DbSet<InPzuResult> InPzuResults { get; set; }
    public DbSet<InPzuOrder> InPzuOrders { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        /*
        modelBuilder.Entity<FinancialResults>()
            .Property(x => x.MinimumAddOn)
            .HasConversion(
            x => $"{x.Value}|{x.Currency}|{x.InPln}",
            x => new Cash(x));
        modelBuilder.Ignore(typeof(Cash));
        */
        modelBuilder.Entity<InPzuPosition>(builder =>
        {
            builder.Property(x => x.PurchaseDate)
                .HasConversion<DateOnlyConverter, DateOnlyComparer>();
        });
        modelBuilder.Entity<InPzuOrder>(builder =>
        {
            builder.Property(x => x.PurchasedOn)
                .HasConversion<DateOnlyConverter, DateOnlyComparer>();
        });
        base.OnModelCreating(modelBuilder);
    }
}
