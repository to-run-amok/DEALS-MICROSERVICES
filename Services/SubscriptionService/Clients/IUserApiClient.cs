public interface IUserApiClient
{
    Task<UserDto?> GetUserAsync(int id);
}