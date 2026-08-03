using Microsoft.EntityFrameworkCore;

public class OrderRepository : IOrderRepository
{
    private readonly OrderDbContext _context;

    public OrderRepository(OrderDbContext context)
    {
        _context = context;
    }
    public async Task<Order?> GetByIdAsync(int id)
    {
        return await _context.DealsOrders.FindAsync(id);
    }
    public async  Task<IEnumerable<Order>> GetByBuyerIdAsync(int buyerId)
    {
        return await _context.DealsOrders
                        .Where(o=>o.BuyerId==buyerId)
                        .ToListAsync();
    }
    public async Task<IEnumerable<Order>> GetByFarmerIdAsync(int farmerId)
    {
        return await _context.DealsOrders
                    .Where(o=>o.FarmerId==farmerId)
                    .ToListAsync();
    }

    public async Task AddAsync(Order order)
    {
        await _context.DealsOrders.AddAsync(order);
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}