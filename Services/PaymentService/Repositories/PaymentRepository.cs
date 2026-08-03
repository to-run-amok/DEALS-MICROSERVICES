using Microsoft.EntityFrameworkCore;

public class PaymentReposiotry : IPaymentRepository
{
    private readonly PaymentDbContext _context;

    public PaymentReposiotry(PaymentDbContext contex)
    {
        _context = contex;
    }

    public async Task<Payment?> GetByIdAsync(int id)
    {
        return await _context.DealsPayments.FindAsync(id);
    }

    public async Task<Payment?> GetByOrderIdAsync(int orderId)
    {
        return await _context.DealsPayments.FirstOrDefaultAsync(p=>p.OrderId == orderId);
    }

    public async Task<IEnumerable<Payment>> GetByBuyerIdAsync(int buyerId)
    {
        return await _context.DealsPayments
                        .Where(p=>p.BuyerId == buyerId)
                        .ToListAsync();
    }

    public async Task<IEnumerable<Payment>> GetByFarmerIdAsync(int farmerId)
    {
        return await _context.DealsPayments
                        .Where(p=>p.FarmerId == farmerId)
                        .ToListAsync();
    }

    public async Task<IEnumerable<Payment>> GetAllPaymentsAsync()
    {
        return await _context.DealsPayments.ToListAsync();
    }

    public async Task AddAsync(Payment payment)
    {
        await _context.DealsPayments.AddAsync(payment);
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}