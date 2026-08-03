using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class CropController : ControllerBase
{
    private readonly ICropService _cropService;

    public CropController(ICropService cropService)
    {
        _cropService = cropService;
    }

    private int CurrentUserId
    {
        get
        {
            var userIdClaims = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if(string.IsNullOrEmpty(userIdClaims) || !int.TryParse(userIdClaims,out int userId))
            {
                throw new UnauthorizedAccessException("User identification missing or invalid");
            }

            return userId;
        }
    }

    
    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> GetAllCrops()
    {
        var crops = await _cropService.GetAllAsync();

        return Ok(crops);
    }

    [HttpGet("{id}")]
    [AllowAnonymous]
    public async Task<IActionResult> GetCrop(int id)
    {
        var crop = await _cropService.GetByIdAsync(id);

        return Ok(crop);
    }
    
    [HttpGet("my-crops")]
    [Authorize(Roles = "Farmer")]
    public async Task<IActionResult> GetCropByFarmer()
    {
        var crop = await _cropService.GetMyCropsAsync(CurrentUserId);

        return Ok(crop);
    }

    [HttpPost]
    [Authorize(Roles = "Farmer")]
    public async Task<IActionResult> CreateCrop([FromBody] CropCreateDto dto)
    {
        var result = await _cropService.CreateAsync(dto,CurrentUserId);

        return CreatedAtAction(nameof(GetCrop), new {id= result.Id},result);
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "Farmer")]
    public async Task<IActionResult> UpdateCrop([FromBody] CropUpdateDto dto, int id)
    {
        var crop = await _cropService.UpdateAsync(id,CurrentUserId,dto);

        return Ok(crop);
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Farmer")]
    public async Task<IActionResult> DeleteCrop(int id)
    {
        await _cropService.DeleteAsync(id,CurrentUserId);

       return NoContent();

    }

    [HttpPut("{id}/sold")]
    [AllowAnonymous]
    public async Task<IActionResult> Sold(int id)
    {
        await _cropService.MarkAsSoldAsync(id);

        return NoContent();
    }
    
}