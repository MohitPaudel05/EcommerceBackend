using System.ComponentModel.DataAnnotations;

namespace Ecommerce.Dtos
{
    public class ProductCreateDto
    {
        [Required, StringLength(100)]
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        [Required]
        public decimal Price { get; set; }
        public IFormFile    ?Image { get; set; }
        public int CategoryId { get; set; }
    }
}
