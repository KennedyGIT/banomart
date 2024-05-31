using System.ComponentModel.DataAnnotations;

namespace Banomart.Services.CouponAPI.Models.DTOs
{
    public class CouponDto
    {
        public long Id { get; set; }
        public string CouponCode { get; set; }
        public double DiscountPercentage { get; set; }
    }
}
