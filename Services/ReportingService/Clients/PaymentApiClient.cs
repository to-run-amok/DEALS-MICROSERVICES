using System.Net.Http.Json;

public class PaymentApiClient : IPaymentApiClient
{
    private readonly HttpClient _httpClient;

    public PaymentApiClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<IEnumerable<PaymentDto>> GetAllPaymentsAsync()
    {
        return await _httpClient.GetFromJsonAsync<IEnumerable<PaymentDto>>(
            "/api/payment/internal/all-payments")
            ?? Enumerable.Empty<PaymentDto>();
    }
}