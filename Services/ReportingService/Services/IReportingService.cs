public interface IReportingService
{
    Task<IEnumerable<CropReportDto>> GetCropReportAsync(GetCropReportQuery query);
    Task<IEnumerable<PaymentReportDto>> GetPaymentReportAsync(GetPaymentReportQuery query);
}