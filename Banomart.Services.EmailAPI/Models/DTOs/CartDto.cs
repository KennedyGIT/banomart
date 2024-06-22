namespace Banomart.Services.EmailAPI.Models
{
    public class CartDto
    {
        public CardHeaderDto CartHeader { get; set; }
        public IEnumerable<CartDetailsDto> CartDetails { get; set; }
    }

    public class CardHeaderDto
    {
        public int CartHeaderId { get; set; }
        public string? UserId { get; set; }
        public string? CouponCode { get; set; }
        public double Discount { get; set; }
        public double CartTotal { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }   
        public string? Email { get; set; }
        public string? Phone {  get; set; }
    }

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
