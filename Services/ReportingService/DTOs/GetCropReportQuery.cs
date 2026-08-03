public class GetCropReportQuery
{
    public string? CropType { get; set; }
    public string? Status { get; set; }
    public string? Location { get; set; }

    public DateTime? FromDate { get; set; }
    public DateTime? ToDate { get; set; }
}
