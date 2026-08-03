public class PaymentDto
{
    public int Id { get; set; }

    public decimal Amount { get; set; }

    public string Status { get; set; } = string.Empty;

    public string TransactionId { get; set; } = string.Empty;

    public string PaymentMethod { get; set; } = string.Empty;

    public DateTime PaidAt { get; set; }
}