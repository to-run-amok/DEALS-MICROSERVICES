public class Paymentservice : IPaymentService
{
    private readonly IPaymentRepository _paymentRepository;
    private readonly IOrderApiClient _orderApiClient;

    public Paymentservice(IPaymentRepository paymentRepository, IOrderApiClient orderApiClient)
    {
        _paymentRepository = paymentRepository;
        _orderApiClient = orderApiClient;
    }

    public async Task<PaymentResponseDto?> GetpaymentByIdAsync(int id)
    {
        var payment = await _paymentRepository.GetByIdAsync(id);

        if(payment == null)
        {
            throw new KeyNotFoundException("The payment listing does not exist.");
        }

        return MapToResponseDto(payment);
    }

    public async Task<IEnumerable<PaymentResponseDto>> GetPaymentByBuyerIdAsync(int buyerId)
    {
        var payments = await _paymentRepository.GetByBuyerIdAsync(buyerId);

        return payments.Select(MapToResponseDto);
    }

    public async Task<IEnumerable<PaymentResponseDto>> GetPaymentByFarmerIdAsync(int farmerId)
    {
        var payments = await _paymentRepository.GetByFarmerIdAsync(farmerId);

        return payments.Select(MapToResponseDto);
    }

    public async Task<IEnumerable<PaymentResponseDto>> GetAllPaymentsAsync()
    {
        var payments = await _paymentRepository.GetAllPaymentsAsync();

        return payments.Select(MapToResponseDto);
    }

    public async Task<PaymentResponseDto> ProcessMockPaymentAsync(PaymentCreateDto dto, int buyerId)
    {
        var order = await _orderApiClient.GetOrderAsync(dto.OrderId);

        
        if(order==null)
        {
            throw new KeyNotFoundException("order not found exist");
        }   

        if(order.BuyerId!=buyerId)
        {
            throw new UnauthorizedAccessException("You may only payfor your orders.");
        }
        var existingPayment = await _paymentRepository.GetByOrderIdAsync(dto.OrderId);

        if(existingPayment!=null)
        {
            throw new InvalidOperationException("Payment already exist for this order");
        }

        if(dto.PaymentMethod==PaymentMethod.Card)
        {
            if(string.IsNullOrWhiteSpace(dto.CardLastFourDigits))
            {
                throw new InvalidOperationException("card last four digits are required.");
            }
            if(dto.CardLastFourDigits.Length!=4)
            {
                throw new InvalidOperationException("card last four digits are required.");
            }
        }
        string mockTransactionId = $"TXN-{Guid.NewGuid().ToString("N")[..8].ToUpper()}";

        
        var payment = new Payment
        {
            OrderId = order.Id,
            CropId = order.CropId,
            BuyerId = order.BuyerId,
            FarmerId = order.FarmerId,
            Amount = order.Amount,
            PaymentMethod = dto.PaymentMethod,
            CardLastFourDigits = dto.CardLastFourDigits,
            Status = PaymentStatus.Success,
            PaidAt = DateTime.UtcNow,
            TransactionId = mockTransactionId

        };

        await _paymentRepository.AddAsync(payment);
        await _paymentRepository.SaveChangesAsync();

        return MapToResponseDto(payment);

    }
    private static PaymentResponseDto MapToResponseDto(Payment payment)
    {
        return new PaymentResponseDto
        {
            Id = payment.Id,
            Amount = payment.Amount,
            Status = payment.Status.ToString(),
            TransactionId = payment.TransactionId,
            PaidAt = payment.PaidAt
        };
    }
}