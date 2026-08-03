public class PaymentResponseDto
{
    public int Id { get; set; }

    public decimal Amount { get; set; }

    public string Status { get; set; } = null!;

    public string TransactionId { get; set; } = null!;

    public DateTime PaidAt { get; set; }
}