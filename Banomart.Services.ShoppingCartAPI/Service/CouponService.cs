using Banomart.Services.ShoppingCartAPI.Models.DTOs;
using Banomart.Services.ShoppingCartAPI.Service.IService;
using Newtonsoft.Json;

namespace Banomart.Services.ShoppingCartAPI.Service
{
    public class CouponService : ICouponService
    {
        private readonly IHttpClientFactory httpClientFactory;

        public CouponService(IHttpClientFactory httpClientFactory)
        {
            this.httpClientFactory = httpClientFactory;
        }

        public async Task<CouponDto> GetCoupon(string couponCode)
        {
            var client = httpClientFactory.CreateClient("Coupon");

            var response = await client.GetAsync($"/api/coupon/GetByCode/{couponCode}");

            var content = await response.Content.ReadAsStringAsync();

            var deserializedResponse = JsonConvert.DeserializeObject<ResponseDto>(content);

            if (deserializedResponse != null && deserializedResponse.IsSuccessful)
            {
                return JsonConvert.DeserializeObject<CouponDto>(Convert.ToString(deserializedResponse.Result));
            }

            return new();
        }
    }
}
