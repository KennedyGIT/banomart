using System.ComponentModel.DataAnnotations;

namespace Banomart.Services.CouponAPI.Models
{
    public class Coupon
    {
        [Key]
        public long Id { get; set; }

        [Required]
        public string CouponCode { get; set; }
        [Required]
        public double DiscountPercentage { get; set; }
    }
}
