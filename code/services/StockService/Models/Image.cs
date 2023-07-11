using System.ComponentModel.DataAnnotations;

namespace StockService.Models
{
    public class Image
    {
        public int Id { get; set; }
        public int StockId { get; set; }
        public string AWSImgUrl { get; set; }
    }
}
