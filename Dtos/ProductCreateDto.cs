using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Ecommerce.Dtos
{
    public class ProductCreateDto
    {
        [Required(ErrorMessage = "Product Name is required")]
        [StringLength(100, ErrorMessage = "Name can't exceed 100 characters")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Product Description is required")]
        public string Description { get; set; } = string.Empty;

        [Required(ErrorMessage = "Product Price is required")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Price must be greater than 0")]
        public decimal Price { get; set; }

        public IFormFile? Image { get; set; }

        [Required(ErrorMessage = "At least one category must be selected")]
        [MinLength(1, ErrorMessage = "At least one category must be selected")]
        public List<int> CategoryIds { get; set; } = new List<int>();
    }
}
