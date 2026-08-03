using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class ReviewController : ControllerBase
{
    private readonly IReviewService _reviewService;

    public ReviewController(IReviewService reviewService)
    {
        _reviewService = reviewService;
    }

     private int CurrentUserId
    {
        get
        {
            var UserClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if(string.IsNullOrEmpty(UserClaim) || !int.TryParse(UserClaim,out int userId))
            {
                throw new UnauthorizedAccessException("User identifdication missing or i nvalid");

            }

            return userId;
        }
    }

    [HttpPost]
    [Authorize(Roles = "Buyer")]
    public async Task<IActionResult> AddReview([FromBody] ReviewCreateDto dto)
    {
        var review = await _reviewService.CreateReviewAsync(dto,CurrentUserId);

        return CreatedAtAction(nameof(GetReviewById),new{id = review.Id},review);
    }

    [HttpGet("{id}")]
    [Authorize]
    public async Task<IActionResult> GetReviewById(int id)
    {
        var review = await _reviewService.GetByIdAsync(id);
        return Ok(review);
    }

    [HttpGet("crop/{cropId}")]
    [AllowAnonymous]
    public async Task<IActionResult> GetReviewByCrops(int cropId)
    {
        
        var reviews = await _reviewService.GetReviewByCropIdAync(cropId);

        return Ok(reviews);

    }
    
    [HttpGet("my-farmer-reviews")]
    [Authorize(Roles = "Farmer")]
    public async Task<IActionResult> GetReviewsByFarmer()
    {
        var reviews = await _reviewService.GetReviewByFarmerIdAync(CurrentUserId);

        return Ok(reviews);
    }

    [HttpGet("my-reviews")]
    [Authorize(Roles = "Buyer")]
    public async Task<IActionResult> GetReviewsByBuyer()
    {
        var reviews = await _reviewService.GetReviewByBuyerIdAync(CurrentUserId);

        return Ok(reviews);
    }

}