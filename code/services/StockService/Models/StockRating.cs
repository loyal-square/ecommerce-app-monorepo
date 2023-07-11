namespace StockService.Models;

public class StockRating
{
    public float RatingValue { get; set; }
    public int UserId { get; set; }
    public DateTime RatedDate { get; set; }
}