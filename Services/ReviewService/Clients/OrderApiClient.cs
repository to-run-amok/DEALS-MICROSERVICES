public class OrderApiClient : IOrderApiClient
{
    private readonly HttpClient _httpClient;

    public OrderApiClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<OrderDto?> GetOrderAsync(int orderId)
    {
        return await _httpClient.GetFromJsonAsync<OrderDto>($"/api/order/{orderId}");
    }
}