using Banomart.Services.CouponAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace Banomart.Services.CouponAPI.Data
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base (options){ }

        public DbSet<Coupon> Coupons { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Coupon>().HasData(
                new Coupon{
                    Id = 1,
                    CouponCode = "10OFF",
                    DiscountPercentage = 10
                },
                new Coupon
                {
                    Id = 2,
                    CouponCode = "20OFF",
                    DiscountPercentage = 20
                }
            );
        }
    }
}
