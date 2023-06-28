using System.ComponentModel.DataAnnotations;

namespace AllYouNeed_Models.DTOS.Requests
{
    public class ProductRegistration
    {
        [Required]
        public string Name { get; set; } = string.Empty;

        [Required]
        public decimal Price { get; set; }

        [Required]
        public long Quantity { get; set; }
    }

}
