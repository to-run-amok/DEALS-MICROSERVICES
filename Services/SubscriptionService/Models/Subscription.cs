public class Subscription
{
    public int Id {get; set;}
    public string CropType {get; set;} = string.Empty;
    public DateTime SubscribedAt {get; set;} = DateTime.UtcNow;

    public int SubscriberId {get; set;}
}