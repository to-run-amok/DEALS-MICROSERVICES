public class CropDto
{
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public decimal Quantity { get; set; }

    public string Status { get; set; } = string.Empty;

    public int FarmerId { get; set; }
}