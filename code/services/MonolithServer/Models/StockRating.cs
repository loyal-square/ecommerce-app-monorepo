namespace MonolithServer.Models;

public class StockRating
{
    public int Id { get; set; }
    public float RatingValue { get; set; }
    public int UserId { get; set; }
    public DateTime RatedDate { get; set; }
    public int StockId { get; set; }
    public int StoreId { get; set; }
}