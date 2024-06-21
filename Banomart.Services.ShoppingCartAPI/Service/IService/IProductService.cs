using Banomart.Services.ShoppingCartAPI.Models.DTOs;

namespace Banomart.Services.ShoppingCartAPI.Service.IService
{
    public interface IProductService
    {
        Task<IEnumerable<ProductDto>> GetProductsAsync();
    }
}
