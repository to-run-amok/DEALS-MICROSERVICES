public class ReportingService : IReportingService
{   

    private readonly ICropApiClient _cropApiClient;
    private readonly IPaymentApiClient _paymentApiClient;

    public ReportingService(ICropApiClient cropApiClient, IPaymentApiClient paymentApiClient)
    {
        _cropApiClient = cropApiClient;
        _paymentApiClient = paymentApiClient;
    }
    public async Task<IEnumerable<CropReportDto>> GetCropReportAsync(GetCropReportQuery query)
    {
        var crops = await _cropApiClient.GetAllCropsAsync();

        if (!string.IsNullOrWhiteSpace(query.CropType))
        {
            crops = crops.Where(x =>
                x.Type.Equals(
                    query.CropType,
                    StringComparison.OrdinalIgnoreCase));
        }

        if (!string.IsNullOrWhiteSpace(query.Location))
        {
            crops = crops.Where(x =>
                x.Location.Equals(
                    query.Location,
                    StringComparison.OrdinalIgnoreCase));
        }

        if (!string.IsNullOrWhiteSpace(query.Status))
        {
            crops = crops.Where(x =>
                x.Status.ToString()
                    .Equals(
                        query.Status,
                        StringComparison.OrdinalIgnoreCase));
        }

        if (query.FromDate.HasValue)
        {
            crops = crops.Where(x =>
                x.PostedAt >= query.FromDate.Value);
        }

        if (query.ToDate.HasValue)
        {
            crops = crops.Where(x =>
                x.PostedAt <= query.ToDate.Value);
        }

        return crops.Select(c => new CropReportDto
        {
            CropId = c.Id,
            CropName = c.Name,
            CropType = c.Type,
            Location = c.Location,
            Status = c.Status.ToString(),
            FarmerId = c.FarmerId,
            PostedAt = c.PostedAt,
            BuyerId = c.BuyerId
    
        });
    }

    public async Task<IEnumerable<PaymentReportDto>>GetPaymentReportAsync(GetPaymentReportQuery query)
    {
        var payments =  await _paymentApiClient.GetAllPaymentsAsync();

        if (query.FromDate.HasValue)
        {
            payments = payments.Where(x =>
                x.PaidAt >= query.FromDate.Value);
        }

        if (query.ToDate.HasValue)
        {
            payments = payments.Where(x =>
                x.PaidAt <= query.ToDate.Value);
        }

        return payments.Select(x => new PaymentReportDto
        {
            PaymentId = x.Id,
            Amount = x.Amount,
            TransactionId = x.TransactionId,
            PaymentMethod = x.PaymentMethod,
            PaidAt = x.PaidAt
        });
    }

}