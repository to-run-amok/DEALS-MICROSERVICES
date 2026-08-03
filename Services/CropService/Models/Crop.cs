public class Crop
{
    public int Id { get; set; }

    public string Type { get; set; } = string.Empty;

    public string Name { get; set; } = string.Empty;

    public decimal Quantity { get; set; }

    public string Location { get; set; } = string.Empty;

    public CropStatus Status { get; set; }

    public DateTime PostedAt { get; set; }

    public int FarmerId { get; set; }

    public decimal? AgreedPrice { get; set; }

    public int? BuyerId { get; set; }
}
