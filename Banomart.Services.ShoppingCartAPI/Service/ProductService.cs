using Banomart.Services.ShoppingCartAPI.Models.DTOs;
using Banomart.Services.ShoppingCartAPI.Service.IService;
using Newtonsoft.Json;

namespace Banomart.Services.ShoppingCartAPI.Service
{
    public class ProducService : IProductService
    {
        private readonly IHttpClientFactory httpClientFactory;

        public ProducService(IHttpClientFactory httpClientFactory)
        {
            this.httpClientFactory = httpClientFactory;
        }


        public async Task<IEnumerable<ProductDto>> GetProductsAsync()
        {
            var client = httpClientFactory.CreateClient("Product");

            var response = await client.GetAsync("api/product");

            var content = await response.Content.ReadAsStringAsync();

            var deserializedResponse = JsonConvert.DeserializeObject<ResponseDto>(content);

            if(deserializedResponse.IsSuccessful) 
            {
                return JsonConvert.DeserializeObject<IEnumerable<ProductDto>>(Convert.ToString(deserializedResponse.Result));
            }

            return new List<ProductDto>();
        }
    }
}
