public interface IPaymentRepository
{
    Task<Payment?> GetByIdAsync(int id);

    Task<Payment?> GetByOrderIdAsync(int orderId);

    Task<IEnumerable<Payment>> GetByBuyerIdAsync(int buyerId);

    Task<IEnumerable<Payment>> GetByFarmerIdAsync(int farmerId);

    Task<IEnumerable<Payment>> GetAllPaymentsAsync();

    Task AddAsync(Payment payment);

    Task SaveChangesAsync();
}