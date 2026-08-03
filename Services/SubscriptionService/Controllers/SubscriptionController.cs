using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "Buyer")]
public class SubscriptionController : ControllerBase
{
    private readonly ISubscriptionService _subscriptionService;

    public SubscriptionController(ISubscriptionService subscriptionService)
    {
        _subscriptionService = subscriptionService;
    }

    private int CurrentUserId
    {
        get
        {
            var UserIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if(string.IsNullOrEmpty(UserIdClaim) || !int.TryParse(UserIdClaim,out int userId))
            {
                throw new UnauthorizedAccessException("User identification missing or invalid.");
            }

            return userId;
        }
    }

    [HttpGet]
    public async Task<IActionResult> MySubscriptions()
    {
        var subs = await _subscriptionService.GetMySubscriptionsAsync(CurrentUserId);

        return Ok(subs);
    }

    [HttpPost]
    public async Task<IActionResult> Subscribe([FromBody] SubcriptionCreateDto dto)
    {
        var sub = await _subscriptionService.SubscribeAsync(dto,CurrentUserId);
        
        return CreatedAtAction(nameof(MySubscriptions),sub);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Unsubscribe(int id)
    {
        await _subscriptionService.UnsubscribeAsync(id,CurrentUserId);

        return NoContent();
    }
}