public interface ISubscriptionRepository
{
    Task AddAsync(Subscription subscription);

    Task<Subscription?> GetByIdAsync(int id);
    
    Task<bool> ExistsAsync(int subscriberId,string cropType);

    Task<IEnumerable<Subscription>> GetBySubscriberIdAsync(int subscriberId);

    void Delete(Subscription subscription);

    Task<IEnumerable<Subscription>> GetByCropTypeAsync(string cropType);
    Task SaveChangesAsync();
}
