using Ecommerce.Models;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.Datas
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
          : base(options) { }

        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure relationship
            modelBuilder.Entity<Category>()
                .HasMany(c => c.Products)
                .WithOne(p => p.Category)
                .HasForeignKey(p => p.CategoryId)
                .OnDelete(DeleteBehavior.Cascade);

            // ✅ Seed Categories
            modelBuilder.Entity<Category>().HasData(
                new Category { Id = 1, Name = "Electronics", Description = "Electronic gadgets and devices" },
                new Category { Id = 2, Name = "Clothing", Description = "All types of clothing" },
                new Category { Id = 3, Name = "Books", Description = "Books of all genres" }
            );

            // ✅ Seed Products
            modelBuilder.Entity<Product>().HasData(
                new Product
                {
                    Id = 1,
                    Name = "Wireless Mouse",
                    Description = "Smooth and responsive wireless mouse",
                    Price = 19.99M,
                    ImageUrl = "https://example.com/mouse.jpg",
                    CategoryId = 1
                },
                new Product
                {
                    Id = 2,
                    Name = "Cotton T-Shirt",
                    Description = "Comfortable cotton t-shirt",
                    Price = 12.50M,
                    ImageUrl = "https://example.com/tshirt.jpg",
                    CategoryId = 2
                },
                new Product
                {
                    Id = 3,
                    Name = "Programming Book",
                    Description = "Learn C# programming",
                    Price = 29.99M,
                    ImageUrl = "https://example.com/book.jpg",
                    CategoryId = 3
                }
            );
        }
    }
}
