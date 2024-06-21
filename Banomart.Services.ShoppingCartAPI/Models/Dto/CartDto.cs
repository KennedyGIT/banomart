namespace Banomart.Services.ShoppingCartAPI.Models.Dto
{
    public class CartDto
    {
        public CardHeaderDto CartHeader { get; set; }
        public IEnumerable<CartDetailsDto> CartDetails { get; set; }
    }
}
