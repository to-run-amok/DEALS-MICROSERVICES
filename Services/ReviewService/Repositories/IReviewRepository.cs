public interface IReviewRepository
{
    Task<Review?> GetByIdAsync(int id);
    Task<IEnumerable<Review>> GetByBuyerIdAsync(int buyerId);
    Task<IEnumerable<Review>> GetByFarmerIdAsync(int farmerd);
    Task<IEnumerable<Review>> GetByCropIdAsync(int cropId);
    Task<Review?> GetByOrderIdAsync(int orderId);
    Task AddAsync(Review review);
    Task SaveChangesAsync();
}