using Microsoft.EntityFrameworkCore;
public class AuthService : IAuthService
{
    private readonly IUserRepository _userRepository;
    private readonly JwtHelper _jwthelper;

    public AuthService(IUserRepository userRepository, JwtHelper jwtHelper)
    {
        _jwthelper = jwtHelper;
        _userRepository = userRepository;
    }

    public async Task<UserResponseDto> RegisterAsync(UserRegisterDto dto)
    {
        var normalizedEmail = dto.Email.Trim().ToLower();

        var existingUser = await _userRepository.GetByEmailAsync(normalizedEmail);

        if(existingUser!=null)
        {
            throw new InvalidOperationException("Email already exists.");
        }

         if(dto.Role == UserRole.Admin)
        {
            throw new InvalidOperationException("Admin registration is not allowed.");
        }

        string hashedPassword = BCrypt.Net.BCrypt.HashPassword(dto.Password);

        var newUser = new User
        {
            Name = dto.Name,
            Email = normalizedEmail,
            PasswordHash = hashedPassword,
            Phone = dto.Phone.Trim(),
            Role = dto.Role,
            CreatedAt = DateTime.UtcNow,
            IsActive = true,
            AccountNumber = dto.AccountNumber,
            BankName = dto.BankName,
            IFSCCode = dto.IFSCCode,

        };

        await _userRepository.AddAsync(newUser);
        await _userRepository.SaveChangesAsync();

        return MapToResponseDto(newUser);
    }

    public async Task<string> LoginAsync(LoginDto dto)
    {   
        var normalizedEmail = dto.Email.Trim().ToLower();
        var user = await _userRepository.GetByEmailAsync(normalizedEmail);

        if(user==null) throw new UnauthorizedAccessException("Inavlid email or person");

        if(!user.IsActive)
        {
            throw new UnauthorizedAccessException("user account inactive");
        }

        bool isPasswordValid = BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash);

        if(!isPasswordValid)
        {
            throw new UnauthorizedAccessException("Invalid email or password.");
        }

        string token = _jwthelper.GenerateToken(user.Id,user.Email,user.Role.ToString());

        return token;
    }

    public async Task<UserResponseDto?> UserProfileAsync(int id)
    {
        var user = await _userRepository.GetByIdAsync(id);

        if(user==null) return null;

        return MapToResponseDto(user);
    }

    public async  Task<UserResponseDto> UpdateProfileAsync(UserUpdateDto dto, int id)
    {
        var user = await _userRepository.GetByIdAsync(id);

        if(user == null)
        {
            throw new KeyNotFoundException("User not found.");
        }

        if(user.Role==UserRole.Admin)
        {
            throw new InvalidOperationException("Updating Admin details is restricted.");
        }
        
        if(!string.IsNullOrWhiteSpace(dto.Name) && !dto.Name.Equals("string"))
        {
            user.Name = dto.Name.Trim();
        }

        if(!string.IsNullOrWhiteSpace(dto.Phone) && !dto.Phone.Equals("string"))
        {
            user.Phone = dto.Phone;
        }

        if(!string.IsNullOrWhiteSpace(dto.AccountNumber))
        {
            user.AccountNumber = dto.AccountNumber;
        }

        if(!string.IsNullOrWhiteSpace(dto.BankName))
        {
            user.BankName = dto.BankName;
        }

        if(!string.IsNullOrWhiteSpace(dto.IFSCCode))
        {
            user.IFSCCode = dto.IFSCCode;
        }

        await _userRepository.SaveChangesAsync();

        return MapToResponseDto(user);
    }
    
    public async Task<UserResponseDto> GetByIdAsync(int id)
    {
        var user = await _userRepository.GetByIdAsync(id);

        if(user==null)
        {
            throw new KeyNotFoundException("User entry not available.");
        }

        return MapToResponseDto(user);

    }
    private static UserResponseDto MapToResponseDto(User user)
    {
        return new UserResponseDto
        {
            Id = user.Id,
            Name = user.Name,
            Email = user.Email,
            Phone = user.Phone,
            Role = user.Role.ToString(),
            CreatedAt = user.CreatedAt,
            AccountNumber = user.AccountNumber??"Unavalible",
            BankName = user.BankName??"Unavalaible",
            IFSCCode = user.IFSCCode??"Unavalaible"
            
        };
    }
}