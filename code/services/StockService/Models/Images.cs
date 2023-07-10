using System.ComponentModel.DataAnnotations;

namespace StockService.Models
{
    public class Images
    {
        public int Id { get; set; }

        [Required]
        public Guid StockId { get; set; }
        public string AWSImgUrl { get; set; }
    }
}
