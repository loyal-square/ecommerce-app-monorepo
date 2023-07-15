namespace StockService.Models
{
    public class StockForServer
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public float Price { get; set; }
        public string Currency { get; set; } = "NZD";
        public string Description { get; set; } = string.Empty;
        public string Details { get; set; } = string.Empty;
        public bool Available { get; set; } = true;
        public int Quantity { get; set; } = 0;
        public int CategoryId { get; set; }
        public int StoreId { get; set; }
        public PriceMultiplier? PriceMultiplier { get; set; } = null;
        public DateTime CreatedDate { get; set; }
        public DateTime? ExpiryDate { get; set; }
    }
}
