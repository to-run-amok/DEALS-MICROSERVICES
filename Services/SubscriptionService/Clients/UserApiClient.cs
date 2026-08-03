using System.Net.Http.Json;

public class UserApiClient : IUserApiClient
{
    private readonly HttpClient _httpClient;

    public UserApiClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<UserDto?> GetUserAsync(int id)
    {
        return await _httpClient.GetFromJsonAsync<UserDto>(
            $"/api/auth/{id}");
    }
}