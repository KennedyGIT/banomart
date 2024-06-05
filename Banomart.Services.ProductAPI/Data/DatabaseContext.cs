using Banomart.Services.ProductAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace Banomart.Services.ProductAPI.Data
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options) { }

        public DbSet<Product> Products { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Product>().HasData(
                new Product
                {
                    Id = 1,
                    Name = "Vintage Canvas Backpack",
                    Price = 49.99,
                    Description = "A durable, vintage-inspired canvas backpack with ample storage for daily essentials.",
                    CategoryName = "Accessories",
                    ImageUrl = "https://cdn.pixabay.com/photo/2019/02/08/15/33/journey-3983404_1280.jpg"
                },
                new Product
                {
                    Id = 2,
                    Name = "Stainless Steel Water Bottle",
                    Price = 19.99,
                    Description = "Eco-friendly, stainless steel water bottle with a leak-proof cap. Keeps drinks hot or cold for hours.",
                    CategoryName = "Home & Kitchen",
                    ImageUrl = "https://cdn.pixabay.com/photo/2017/01/23/09/52/water-2001912_1280.jpg"
                },
                new Product
                {
                    Id = 3,
                    Name = "Ergonomic Wireless Mouse",
                    Price = 29.95,
                    Description = "Comfortable wireless mouse with customizable buttons and adjustable DPI for precision control.",
                    CategoryName = "Electronics",
                    ImageUrl = "https://cdn.pixabay.com/photo/2017/04/07/13/13/wireless-mouse-2210970_1280.jpg"
                },
                new Product
                {
                    Id = 4,
                    Name = "Organic Green Tea Leaves",
                    Price = 15.50,
                    Description = "A pack of premium, organic green tea leaves rich in antioxidants and natural flavors.",
                    CategoryName = "Groceries",
                    ImageUrl = "https://cdn.pixabay.com/photo/2021/03/08/06/23/green-tea-6078275_1280.jpg"
                },
                new Product
                {
                    Id = 5,
                    Name = "Yoga Mat with Carrying Strap",
                    Price = 35.00,
                    Description = "Non-slip yoga mat with a comfortable thickness and a carrying strap for easy transport.",
                    CategoryName = "Fitness",
                    ImageUrl = "https://cdn.pixabay.com/photo/2016/10/15/18/29/yoga-mat-1743203_1280.jpg"
                }

            );
        }
    }
}
