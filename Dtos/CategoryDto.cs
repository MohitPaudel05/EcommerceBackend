using System.Collections.Generic;

namespace Ecommerce.Dtos
{
    public class CategoryDto
    {
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public string? Description { get; set; }

        public List<string> ProductNames { get; set; } = new List<string>();
    }
}
