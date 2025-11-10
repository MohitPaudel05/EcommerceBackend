using System.ComponentModel.DataAnnotations;

namespace Ecommerce.Dtos
{
    public class CategoryCreateDto
    {
        [Required(ErrorMessage = "Category Name is required")]
        [StringLength(100, ErrorMessage = "Name can't exceed 100 characters")]
        public string Name { get; set; } = string.Empty;

        public string? Description { get; set; }
    }
}
