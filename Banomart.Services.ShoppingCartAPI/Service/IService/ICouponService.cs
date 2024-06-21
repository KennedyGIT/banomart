using Banomart.Services.ShoppingCartAPI.Models.DTOs;

namespace Banomart.Services.ShoppingCartAPI.Service.IService
{
    public interface ICouponService
    {
        Task<CouponDto> GetCoupon(string coupon);
    }
}
