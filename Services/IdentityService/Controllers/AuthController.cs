using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }


    private int CurrentUserId
    {
        get
        {
            var userIdclaims = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if(string.IsNullOrEmpty(userIdclaims) || !int.TryParse(userIdclaims,out int userId))
            {
                throw new UnauthorizedAccessException("User identification missing or invalid");
            }

            return userId;
        }
    }

    [HttpPost("register")]
    [AllowAnonymous]
    public async Task<IActionResult> register([FromBody] UserRegisterDto dto)
    {
        var user = await _authService.RegisterAsync(dto);

        return Ok(user);
    }

    [HttpPost("login")]
    [AllowAnonymous]
    public async Task<IActionResult> login([FromBody] LoginDto dto)
    {
        var token = await _authService.LoginAsync(dto);

        return Ok(token);
    }

    [HttpGet("profile")]
    [Authorize]
    public async Task<IActionResult> profile()
    {   
        var user = await _authService.UserProfileAsync(CurrentUserId);

        return Ok(user);
    }

    [HttpPut("profile")]
    [Authorize]
    public async Task<IActionResult> update([FromBody] UserUpdateDto dto)
    {
        var user = await _authService.UpdateProfileAsync(dto,CurrentUserId);

        return Ok(user);
    }

    [HttpGet("{id}")]
    [AllowAnonymous]
    public async Task<IActionResult> GetUser(int id)
    {
        var user = await _authService.GetByIdAsync(id);

        return Ok(user);
    }
}