public interface IPaymentService
{
    Task<PaymentResponseDto?> GetpaymentByIdAsync(int id);
    Task<IEnumerable<PaymentResponseDto>> GetPaymentByBuyerIdAsync(int buyerId);
    Task<IEnumerable<PaymentResponseDto>> GetPaymentByFarmerIdAsync(int farmerId);
    Task<IEnumerable<PaymentResponseDto>> GetAllPaymentsAsync();
    Task<PaymentResponseDto> ProcessMockPaymentAsync(PaymentCreateDto dto, int buyerId);
}