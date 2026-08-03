public class OrderDto
{
    public int Id { get; set; }

    public int BuyerId { get; set; }

    public int FarmerId { get; set; }

    public int CropId { get; set; }

    public decimal Amount { get; set; }
}