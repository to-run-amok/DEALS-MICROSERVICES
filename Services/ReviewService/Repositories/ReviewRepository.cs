using Microsoft.EntityFrameworkCore;

public class ReviewReposiotry : IReviewRepository
{
    private readonly ReviewDbContext _context;

    public ReviewReposiotry(ReviewDbContext contex)
    {
        _context = contex;
    }

    public async Task<Review?> GetByIdAsync(int id)
    {
        return await _context.DealsReviews.FindAsync(id);
    }

    public async Task<IEnumerable<Review>> GetByBuyerIdAsync(int buyerId)
    {
        return await _context.DealsReviews
                        .Where(p=>p.BuyerId == buyerId)
                        .ToListAsync();
    }

    public async Task<IEnumerable<Review>> GetByFarmerIdAsync(int farmerId)
    {
        return await _context.DealsReviews
                        .Where(p=>p.FarmerId == farmerId)
                        .ToListAsync();
    }

    public async Task<IEnumerable<Review>> GetByCropIdAsync(int cropId)
    {
        return await _context.DealsReviews
                        .Where(p=>p.CropId==cropId)
                        .ToListAsync();
    }

    public async Task<Review?> GetByOrderIdAsync(int orderId)
    {
        return await _context.DealsReviews
                        .FirstOrDefaultAsync(p=>p.OrderId==orderId);
    }

    public async Task AddAsync(Review review)
    {
        await _context.DealsReviews.AddAsync(review);
    }
    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}