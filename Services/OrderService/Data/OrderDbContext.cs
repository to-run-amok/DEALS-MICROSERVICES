using Microsoft.EntityFrameworkCore;

public class OrderDbContext : DbContext
{
    public OrderDbContext(DbContextOptions<OrderDbContext> options) : base (options)
    {
        
    }

    public DbSet<Order> DealsOrders {get; set;}

    protected override void OnModelCreating(ModelBuilder builder)
    {
            builder.Entity<Order>()
                .Property(o => o.Amount)
                .HasPrecision(18,2);

            builder.Entity<Order>()
                .Property(o => o.Quantity)
                .HasPrecision(18,2);

            builder.Entity<Order>()
                .Property(o => o.AgreedPrice)
                .HasPrecision(18,2);

    }
}