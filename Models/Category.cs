using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Ecommerce.Models
{
    public class Category
    {
        public int Id { get; set; }

        [Required, StringLength(100)]
        public string Name { get; set; }

        public string? Description { get; set; }

        [JsonIgnore]
        public ICollection<Product> Products { get; set; } = new List<Product>();
    }
}
