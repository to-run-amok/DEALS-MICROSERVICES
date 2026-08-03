public interface IPaymentApiClient
{
    Task<IEnumerable<PaymentDto>> GetAllPaymentsAsync();
}