using Microsoft.EntityFrameworkCore;

public class CropDbContext : DbContext
{
    public CropDbContext(DbContextOptions<CropDbContext> options) : base(options)
    {
        
    }

    public DbSet<Crop> DealsCrops {get; set;}

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Crop>()
            .Property(x => x.AgreedPrice)
            .HasPrecision(18,2);

        
       modelBuilder.Entity<Crop>()
            .Property(x => x.Quantity)
            .HasPrecision(18,2);

    }
}