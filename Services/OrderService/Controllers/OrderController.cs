using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class OrderController : ControllerBase
{
    private readonly IOrderService _orderService;

    public OrderController(IOrderService orderService)
    {
        _orderService = orderService;
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

    [HttpPost]
    [Authorize(Roles = "Buyer")]
    public async Task<IActionResult> purchaseCrop([FromBody] OrderCreateDto dto)
    {
        var order = await _orderService.CreateOrderAsync(dto,CurrentUserId);

        return CreatedAtAction(nameof(GetOrderbyId),new {id = order.Id},order);
    }

    [HttpGet("{id}")]
    [AllowAnonymous]
    public async Task<IActionResult> GetOrderbyId(int id)
    {
        var order = await _orderService.GetOrderByIdAsync(id);

        return Ok(order);
    }

    [HttpGet("my-orders")]
    [Authorize(Roles = "Buyer")]
    public async Task<IActionResult> GetMyOrders()
    {
        var orders = await _orderService.GetOrdersByBuyerIdAsync(CurrentUserId);

        return Ok(orders);
    }

    [HttpGet("farmer-orders")]
    [Authorize(Roles = "Farmer")]
    public async Task<IActionResult> GetFarmerOrders()
    {
        var orders = await _orderService.GetOrdersByFarmerIdAsync(CurrentUserId);

        return Ok(orders);
    }
    
}