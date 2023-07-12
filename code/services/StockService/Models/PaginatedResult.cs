namespace StockService.Models;

public class PaginatedResult
{
    public dynamic PaginatedItems { get; set; }
    public int TotalPages { get; set; }
    public int TotalItems { get; set; }
}