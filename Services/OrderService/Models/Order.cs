public class Order
{
    public int Id { get; set; }

    public int CropId { get; set; }

    public int FarmerId { get; set; }

    public int BuyerId { get; set; }

    public string CropName { get; set; } = null!;

    public decimal Quantity { get; set; }

    public decimal Amount { get; set; }

    public decimal AgreedPrice { get; set; }

    public DateTime GeneratedAt { get; set; }
}