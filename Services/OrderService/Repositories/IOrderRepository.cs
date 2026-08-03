public interface IOrderRepository
{
    Task AddAsync(Order order);

    Task<Order?> GetByIdAsync(int id);

    Task<IEnumerable<Order>> GetByBuyerIdAsync(int buyerId);

    Task<IEnumerable<Order>> GetByFarmerIdAsync(int farmerId);

    Task SaveChangesAsync();
}