using Microsoft.EntityFrameworkCore;

public class PaymentDbContext : DbContext
{
    public PaymentDbContext(DbContextOptions<PaymentDbContext> options) : base (options)
    {
        
    }

    public DbSet<Payment> DealsPayments {get; set;}

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
            modelBuilder.Entity<Payment>()
                    .Property(x => x.Amount)
                    .HasPrecision(18,2);
    }
}