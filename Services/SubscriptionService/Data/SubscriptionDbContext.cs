using Microsoft.EntityFrameworkCore;

public class SubcriptionDbContext : DbContext
{
    public SubcriptionDbContext(DbContextOptions<SubcriptionDbContext> options) : base(options)
    {
        
    }

    public DbSet<Subscription> DealsSubscriptions {get; set;}
}