using System.ComponentModel.DataAnnotations;

namespace bano_mart_mvc.Models
{
    public class CouponDto
    {
        public long Id { get; set; }

        [Display(Name = "Coupon Code")]
        public string CouponCode { get; set; }


        [Display(Name = "Discount Percentage")]
        public double DiscountPercentage { get; set; }
    }
}
