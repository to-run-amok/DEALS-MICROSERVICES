using System.Net.Http.Json;

public class CropApiClient : ICropApiClient
{
    private readonly HttpClient _httpClient;

    public CropApiClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<IEnumerable<CropDto>> GetAllCropsAsync()
    {
        return await _httpClient.GetFromJsonAsync<IEnumerable<CropDto>>(
            "/api/crop")
            ?? Enumerable.Empty<CropDto>();
    }
}