public class CropReportDto
{
    public int CropId { get; set; }

    public string CropName { get; set; } = string.Empty;

    public string CropType { get; set; } = string.Empty;

    public string Location { get; set; } = string.Empty;

    public string Status { get; set; } = string.Empty;

    public DateTime PostedAt { get; set; }

    public int FarmerId { get; set; }

    public int? BuyerId {get; set;}
}