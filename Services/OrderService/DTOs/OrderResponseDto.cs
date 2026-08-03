public class OrderResponseDto
{
    public int Id { get; set; }

    public int CropId { get; set; }

    public string CropName { get; set; } = string.Empty;

    public decimal Quantity { get; set; }

    public decimal Amount { get; set; }

    public DateTime GeneratedAt { get; set; }

    public int BuyerId { get; set; }

    public int FarmerId { get; set; }
}