public class Review
{
    public int Id { get; set; }
    public int Rating { get; set; }          
    public string Comment { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } 
    public int BuyerId { get; set; }
    public int CropId { get; set; }
    public int OrderId {get; set;}
    public int FarmerId { get; set; }

}