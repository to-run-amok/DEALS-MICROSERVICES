using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "Admin")]
public class ReportingController : ControllerBase
{   
    private readonly IReportingService _reportingService;

    public ReportingController( IReportingService reportingService)
    {
        _reportingService = reportingService;
    }
    
    
    [HttpGet("Crops")]
    public async Task<IActionResult> GetCropReport([FromQuery] GetCropReportQuery query)
    {
        var crops = await _reportingService.GetCropReportAsync(query);

        return Ok(crops);
    }

    [HttpGet("Payments")]
    public async Task<IActionResult> GetPaymentReport([FromQuery] GetPaymentReportQuery query)
    {
        var payments = await _reportingService.GetPaymentReportAsync(query);

        return Ok(payments);
    }
}