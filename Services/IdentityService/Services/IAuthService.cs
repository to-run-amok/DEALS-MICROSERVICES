public interface IAuthService
{
    Task<UserResponseDto> RegisterAsync(UserRegisterDto dto);

    Task<string> LoginAsync(LoginDto dto);

    Task<UserResponseDto?> UserProfileAsync(int id);

    Task<UserResponseDto> UpdateProfileAsync(UserUpdateDto dto, int id);

    Task<UserResponseDto> GetByIdAsync(int id);
}