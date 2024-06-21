using Banomart.Services.ShoppingCartAPI.Models.DTOs;

namespace Banomart.Services.ShoppingCartAPI.Models.Dto
{
    public class CartDetailsDto
    {
        public int? CartDetailsId { get; set; }
        public int? CartHeaderId { get; set; }
        public CardHeaderDto? CartHeader { get; set; }
        public int? ProductId { get; set; }
        public ProductDto? Product { get; set; }
        public int Quantity { get; set; }
    }
}
