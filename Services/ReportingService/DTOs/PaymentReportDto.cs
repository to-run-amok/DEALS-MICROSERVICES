public class PaymentReportDto
{
    public int PaymentId { get; set; }

    public decimal Amount { get; set; }

    public string TransactionId { get; set; } = string.Empty;

    public string PaymentMethod { get; set; } = string.Empty;

    public DateTime PaidAt { get; set; }
}