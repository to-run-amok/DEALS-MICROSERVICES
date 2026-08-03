using Microsoft.EntityFrameworkCore;

public class SubscriptionRepository : ISubscriptionRepository
{
    private readonly SubcriptionDbContext _context;

    public SubscriptionRepository(SubcriptionDbContext context)
    {
        _context = context;
    }

    public async Task<Subscription?> GetByIdAsync(int id)
    {
        return await _context.DealsSubscriptions.FindAsync(id);
    }

    public async Task<IEnumerable<Subscription>> GetBySubscriberIdAsync(int subscriberId)
    {
        return await _context.DealsSubscriptions
                .Where(s => s.SubscriberId == subscriberId)
                .ToListAsync();
    }
    public async Task<bool> ExistsAsync(int subscriberId,string cropType)
    {
        var subs = await _context.DealsSubscriptions
                                .Where(s => s.CropType == cropType && s.SubscriberId == subscriberId)
                                .FirstOrDefaultAsync();

        if(subs==null)return false;

        return true;
    }

    public async Task<IEnumerable<Subscription>> GetByCropTypeAsync(string cropType)
    {
        return await _context.DealsSubscriptions
                        .Where(s=> s.CropType == cropType)
                        .ToListAsync();
    }

    public async Task AddAsync(Subscription subscription)
    {
        await _context.DealsSubscriptions.AddAsync(subscription);
    }

    public void Delete(Subscription subscription)
    {
        _context.DealsSubscriptions.Remove(subscription);
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }

    
    
}