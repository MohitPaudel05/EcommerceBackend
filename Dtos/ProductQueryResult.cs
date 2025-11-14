namespace Ecommerce.Dtos
{
    public class ProductQueryResult
    {
        public int Id { get; set; }               
        public string?Name { get; set; } = "";    
        public string?Description { get; set; } = "";
        public decimal? Price { get; set; }       
        public string? ImageUrl { get; set; }
        public string? CategoryNames { get; set; } // Comma separated category names
    }
}
