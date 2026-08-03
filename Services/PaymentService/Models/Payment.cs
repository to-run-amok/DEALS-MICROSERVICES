public class Payment
{
    public int Id { get; set; }

    public decimal Amount { get; set; }

    public PaymentMethod PaymentMethod { get; set; }

    public string? CardLastFourDigits { get; set; }

    public PaymentStatus Status { get; set; } 

    public string TransactionId { get; set; } = null!;

    public DateTime PaidAt { get; set; }

    public int BuyerId { get; set; }

    public int FarmerId { get; set; }

    public int CropId { get; set; }

    public int OrderId { get; set; }
}