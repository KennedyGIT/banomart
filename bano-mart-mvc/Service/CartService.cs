using bano_mart_mvc.Models;
using bano_mart_mvc.Service.IService;
using bano_mart_mvc.Utility;

namespace bano_mart_mvc.Service
{
    public class CartService : ICartService
    {
        private readonly IBaseService baseService;

        public CartService(IBaseService baseService)
        {
            this.baseService = baseService;
        }

        public async Task<ResponseDto?> ApplyCouponAsync(CartDto cartDto)
        {
            return await baseService.SendAsync(new RequestDto() 
            {
                HttpMethod = Enums.HttpMethod.POST,
                Data = cartDto,
                Url = Common.CartAPIBase + "/api/cart/ApplyCoupon"
            });
        }

        public async Task<ResponseDto?> EmailCart(CartDto cartDto)
        {
            return await baseService.SendAsync(new RequestDto()
            {
                HttpMethod = Enums.HttpMethod.POST,
                Data = cartDto,
                Url = Common.CartAPIBase + "/api/cart/EmailCart"
            });
        }

        public async Task<ResponseDto?> GetCartByUserIdAsync(string userId)
        {
            return await baseService.SendAsync(new RequestDto()
            {
                HttpMethod = Enums.HttpMethod.GET,
                Url = Common.CartAPIBase + "/api/cart/GetCart/" + userId
            });
        }    

        public async Task<ResponseDto?> RemoveFromCartAsync(int cartDetailsId)
        {
            return await baseService.SendAsync(new RequestDto() 
            {
                HttpMethod = Enums.HttpMethod.POST,
                Data = cartDetailsId,
                Url = Common.CartAPIBase + "/api/cart/RemoveCart"
            });
        }

        public async Task<ResponseDto?> UpsertCartAsync(CartDto cartDto)
        {
            return await baseService.SendAsync(new RequestDto()
            {
                HttpMethod = Enums.HttpMethod.POST,
                Data = cartDto,
                Url = Common.CartAPIBase + "/api/cart/CartUpSert"
            });
        }
    }
}
