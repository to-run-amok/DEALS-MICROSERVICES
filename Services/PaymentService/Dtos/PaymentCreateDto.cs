public class PaymentCreateDto
{
    public int OrderId { get; set; }

    public PaymentMethod PaymentMethod { get; set; }

    public string? CardLastFourDigits { get; set; }
}
