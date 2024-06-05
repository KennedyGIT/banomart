using bano_mart_mvc.Models;
using bano_mart_mvc.Service.IService;
using bano_mart_mvc.Utility;

namespace bano_mart_mvc.Service
{
    public class ProductService : IProductService
    {
        private readonly IBaseService baseService;

        public ProductService(IBaseService baseService)
        {
            this.baseService = baseService;
        }

        public async Task<ResponseDto?> CreateProductAsync(ProductDto productDto)
        {
            return await baseService.SendAsync(new RequestDto()
            {
                HttpMethod = Enums.HttpMethod.POST,
                Data = productDto,
                Url = Common.ProductAPIBase + "/api/product"
            });
        }

        public async Task<ResponseDto?> DeleteProductAsync(int id)
        {
            return await baseService.SendAsync(new RequestDto()
            {
                HttpMethod = Enums.HttpMethod.DELETE,
                Url = Common.ProductAPIBase + "/api/product/" + id
            });
        }

        public async Task<ResponseDto?> GetAllProductsAsync()
        {
            return await baseService.SendAsync(new RequestDto()
            {
                HttpMethod = Enums.HttpMethod.GET,
                Url = Common.ProductAPIBase + "/api/product"
            });
        }

        public async Task<ResponseDto?> GetProdyctByIdAsync(int id)
        {
            return await baseService.SendAsync(new RequestDto()
            {
                HttpMethod = Enums.HttpMethod.GET,
                Url = Common.ProductAPIBase + "/api/product/" + id
            });
        }

        public async Task<ResponseDto?> UpdateProductAsync(ProductDto productDto)
        {
            return await baseService.SendAsync(new RequestDto()
            {
                HttpMethod = Enums.HttpMethod.PUT,
                Data = productDto,
                Url = Common.ProductAPIBase + "/api/product"
            });
        }
    }
}
