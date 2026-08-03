public class CropCreatedEvent
{
    public int CropId { get; set; }

    public string CropType { get; set; } = string.Empty;

    public string CropName { get; set; } = string.Empty;

    public int FarmerId { get; set; }
}