public interface IOrderService
{
    Task<OrderResponseDto> GetOrderByIdAsync(int id);
    Task<IEnumerable<OrderResponseDto>> GetOrdersByBuyerIdAsync(int buyerId);
    Task<IEnumerable<OrderResponseDto>> GetOrdersByFarmerIdAsync(int farmerId);
    Task<OrderResponseDto> CreateOrderAsync(OrderCreateDto dto, int buyerId);
}