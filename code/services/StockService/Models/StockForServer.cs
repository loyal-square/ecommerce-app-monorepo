namespace StockService.Models
{
    public class StockForServer
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Currency { get; set; }
        public string Description { get; set; }
        public string Details { get; set; }
        public bool Available { get; set; }
        public int Quantity { get; set; }
        public int CategoryId { get; set; }
        public int StoreId { get; set; }
        public StockRating[] RatingsArray { get; set; }
        public PriceMultiplier PriceMultiplier { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ExpiryDate { get; set; }
    }
}
