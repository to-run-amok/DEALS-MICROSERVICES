public interface IOrderApiClient
{
    Task<OrderDto?> GetOrderAsync(int orderId);
}