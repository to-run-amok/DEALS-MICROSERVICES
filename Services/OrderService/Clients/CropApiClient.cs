public class CropApiClient : ICropApiClient
{
    private readonly HttpClient _httpClient;

    public CropApiClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<CropDto?> GetCropAsync(int cropId)
    {
        return await _httpClient.GetFromJsonAsync<CropDto>($"/api/crop/{cropId}");
    }

    public async Task MarkAsSoldAsync(int cropId)
    {
        var response = await _httpClient.PutAsync($"/api/crop/{cropId}/sold",null);

        response.EnsureSuccessStatusCode();
    }
}