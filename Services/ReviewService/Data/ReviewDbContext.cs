using Microsoft.EntityFrameworkCore;

public class ReviewDbContext : DbContext
{
    public ReviewDbContext(DbContextOptions<ReviewDbContext> options ) : base(options)
    {
        
    }

    public DbSet<Review> DealsReviews {get; set;}

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Review>()
                .ToTable(t =>t.HasCheckConstraint("CK_Review_Rating","[Rating] >= 1 AND [Rating] <= 5"));
    }
}