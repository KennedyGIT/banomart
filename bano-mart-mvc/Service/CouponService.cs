using bano_mart_mvc.Models;
using bano_mart_mvc.Service.IService;
using bano_mart_mvc.Utility;

namespace bano_mart_mvc.Service
{
    public class CouponService : ICouponService
    {
        private readonly IBaseService baseService;

        public CouponService(IBaseService baseService)
        {
            this.baseService = baseService;
        }

        public async Task<ResponseDto?> CreateCouponsAsync(CouponDto couponDto)
        {
            return await baseService.SendAsync(new RequestDto()
            {
                HttpMethod = Enums.HttpMethod.POST,
                Data = couponDto,
                Url = Common.CouponAPIBase + "/api/coupon"
            });
        }

        public async Task<ResponseDto?> DeleteCouponsAsync(int id)
        {
            return await baseService.SendAsync(new RequestDto()
            {
                HttpMethod = Enums.HttpMethod.DELETE,
                Url = Common.CouponAPIBase + "/api/coupon/" + id
            });
        }
    

        public async Task<ResponseDto?> GetAllCouponAsync()
        {
            return await baseService.SendAsync(new RequestDto()
            {   
                HttpMethod = Enums.HttpMethod.GET,
                Url = Common.CouponAPIBase + "/api/coupon"
            });
        }

        public async Task<ResponseDto?> GetCouponAsync(string couponCode)
        {
            return await baseService.SendAsync(new RequestDto()
            {
                HttpMethod = Enums.HttpMethod.GET,
                Url = Common.CouponAPIBase + "/api/coupon/GetByCode/" + couponCode
            });
        }

        public async Task<ResponseDto?> GetCouponByIdAsync(int id)
        {
            return await baseService.SendAsync(new RequestDto()
            {
                HttpMethod = Enums.HttpMethod.GET,
                Url = Common.CouponAPIBase + "/api/coupon/" + id 
            });
        }

        public async Task<ResponseDto?> UpdateCouponsAsync(CouponDto couponDto)
        {
            return await baseService.SendAsync(new RequestDto()
            {
                HttpMethod = Enums.HttpMethod.PUT,
                Data = couponDto,
                Url = Common.CouponAPIBase + "/api/coupon"
            });
        }
    }
}
