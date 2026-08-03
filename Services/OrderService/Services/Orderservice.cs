
public class Orderservice : IOrderService
{
    private readonly IOrderRepository _orderRepo;
    private readonly RabbitMqPublisher _publisher;
    private readonly ICropApiClient _cropApiClient;

    public Orderservice(IOrderRepository orderRepo, ICropApiClient cropApiClient,RabbitMqPublisher publisher)
    {
        _orderRepo = orderRepo;
        _cropApiClient = cropApiClient;
        _publisher = publisher;
    }
    public async Task<OrderResponseDto> GetOrderByIdAsync(int id)
    {
        var order = await _orderRepo.GetByIdAsync(id);

        if(order==null)
        {
            throw new KeyNotFoundException("The order record does not exist.");
        }

        return MapToResponseDto(order);
    }
    public async 
    Task<IEnumerable<OrderResponseDto>> GetOrdersByBuyerIdAsync(int buyerId)
    {
        var orders = await _orderRepo.GetByBuyerIdAsync(buyerId);

        return orders.Select(MapToResponseDto);
    }

    public async Task<IEnumerable<OrderResponseDto>> GetOrdersByFarmerIdAsync(int farmerId)
    {
        var orders = await _orderRepo.GetByFarmerIdAsync(farmerId);

        return orders.Select(MapToResponseDto);
    }   

    public async Task<OrderResponseDto> CreateOrderAsync(OrderCreateDto dto, int buyerId)
    {
        var crop = await _cropApiClient.GetCropAsync(dto.CropId);

        if(crop==null)
        {
            throw new KeyNotFoundException("crop not found.");
        }
        if(crop.FarmerId==buyerId)
        {
            throw new InvalidOperationException("You cannot place an order for your own crop.");
        }
        if(crop.Status!="Avaliable")
        {
            throw new InvalidOperationException("The crop listing has been sold.");
        }  
        if(dto.Quantity <= 0)
        {
            throw new InvalidOperationException("Quantity must be greater than zero.");
        }
        if(dto.AgreedPrice <= 0)
        {
            throw new InvalidOperationException("Agreed price must be greater than zero.");
        }
        if(dto.Quantity > crop.Quantity)
        {
            throw new InvalidOperationException("Insufficient crop quantity available.");}

        decimal amount = dto.AgreedPrice*dto.Quantity;

        Order neword = new Order
        {
            CropId = dto.CropId,
            FarmerId = crop.FarmerId,
            BuyerId = buyerId,
            CropName = crop.Name,
            Quantity = dto.Quantity,
            Amount = amount,
            AgreedPrice = dto.AgreedPrice,
            GeneratedAt = DateTime.UtcNow
        };

        await _orderRepo.AddAsync(neword);
        await _orderRepo.SaveChangesAsync();

        await _publisher.PublishAsync("orders-created",
                    new OrderCreatedEvent
                    {
                        OrderId = neword.Id,
                        CropId = neword.CropId,
                        BuyerId = neword.BuyerId
                    });

        return MapToResponseDto(neword);
    }

    private static OrderResponseDto MapToResponseDto(Order order)
    {
        return new OrderResponseDto
        {   Id = order.Id,
            CropId = order.CropId,
            CropName = order.CropName,
            Quantity = order.Quantity,
            Amount = order.Amount,
            GeneratedAt = order.GeneratedAt,
            FarmerId = order.FarmerId,
            BuyerId = order.BuyerId
        };
    }
}