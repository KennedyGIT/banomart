namespace Banomart.Services.ShoppingCartAPI.Models.DTOs
{
    public class CouponDto
    {
        public long Id { get; set; }
        public string CouponCode { get; set; }
        public double DiscountPercentage { get; set; }
    }
}
