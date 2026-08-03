using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class PaymentController : ControllerBase
{
    private readonly IPaymentService _paymentService;

    public PaymentController(IPaymentService paymentService)
    {
        _paymentService = paymentService;
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
    public async Task<IActionResult> MakePayment([FromBody] PaymentCreateDto dto)
    {
        var payment = await _paymentService.ProcessMockPaymentAsync(dto,CurrentUserId);

        return Ok(payment);
    }

    [HttpGet("{id}")]
    [Authorize]
    public async Task<IActionResult> GetPaymentDetails(int id)
    {
        var payment = await _paymentService.GetpaymentByIdAsync(id);

        if(payment == null) return NotFound("payment record not found");

        return Ok(payment);
    }

    [HttpGet("my-payments")]
    [Authorize(Roles = "Buyer")]
    public async Task<IActionResult> GetMyPayments()
    {
        var payments = await _paymentService.GetPaymentByBuyerIdAsync(CurrentUserId);

        return Ok(payments);
    }

    [HttpGet("farmer-payments")]
    [Authorize(Roles = "Farmer")]
    public async Task<IActionResult> GetFarmerPayments()
    {
        var payments = await _paymentService.GetPaymentByFarmerIdAsync(CurrentUserId);

        return Ok(payments);
    }

    [HttpGet("internal/all-payments")]
    [AllowAnonymous]
    public async Task<IActionResult> GetAllPayments()
    {
        var payments = await _paymentService.GetAllPaymentsAsync();

        return Ok(payments);
    }
}