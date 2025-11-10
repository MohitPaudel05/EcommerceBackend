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
        public DbSet<ProductCategory> ProductCategories { get; set; } // new join table

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure decimal precision for Price
            modelBuilder.Entity<Product>()
                .Property(p => p.Price)
                .HasPrecision(18, 2); // <-- added this line

            // Configure many-to-many relationship
            modelBuilder.Entity<ProductCategory>()
                .HasKey(pc => new { pc.ProductId, pc.CategoryId });

            modelBuilder.Entity<ProductCategory>()
                .HasOne(pc => pc.Product)
                .WithMany(p => p.ProductCategories)
                .HasForeignKey(pc => pc.ProductId);

            modelBuilder.Entity<ProductCategory>()
                .HasOne(pc => pc.Category)
                .WithMany(c => c.ProductCategories)
                .HasForeignKey(pc => pc.CategoryId);

            // Seed Categories
            modelBuilder.Entity<Category>().HasData(
                new Category { Id = 1, Name = "Electronics", Description = "Electronic gadgets" },
                new Category { Id = 2, Name = "Clothing", Description = "All types of clothing" },
                new Category { Id = 3, Name = "Books", Description = "Books of all genres" }
            );

            // Seed Products
            modelBuilder.Entity<Product>().HasData(
                new Product
                {
                    Id = 1,
                    Name = "Wireless Mouse",
                    Description = "Smooth and responsive wireless mouse",
                    Price = 19.99M,
                    ImageUrl = "https://example.com/mouse.jpg"
                },
                new Product
                {
                    Id = 2,
                    Name = "Cotton T-Shirt",
                    Description = "Comfortable cotton t-shirt",
                    Price = 12.50M,
                    ImageUrl = "https://example.com/tshirt.jpg"
                },
                new Product
                {
                    Id = 3,
                    Name = "Programming Book",
                    Description = "Learn C# programming",
                    Price = 29.99M,
                    ImageUrl = "https://example.com/book.jpg"
                }
            );

            // Seed ProductCategory (assign multiple categories)
            modelBuilder.Entity<ProductCategory>().HasData(
                new ProductCategory { ProductId = 1, CategoryId = 1 },
                new ProductCategory { ProductId = 2, CategoryId = 2 },
                new ProductCategory { ProductId = 3, CategoryId = 3 },
                new ProductCategory { ProductId = 3, CategoryId = 1 } // Product 3 also in Electronics
            );
        }
    }
}