using AutoMapper;
using Banomart.Services.ShoppingCartAPI.Data;
using Banomart.Services.ShoppingCartAPI.Models;
using Banomart.Services.ShoppingCartAPI.Models.Dto;
using Banomart.Services.ShoppingCartAPI.Models.DTOs;
using Banomart.Services.ShoppingCartAPI.Service.IService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Banomart.Services.ShoppingCartAPI.Controllers
{
    [Route("api/cart")]
    [ApiController]
    [Authorize]
    public class CartAPIController : ControllerBase
    {
        private ResponseDto responseDto;
        private IMapper mapper;
        private readonly IProductService productService;
        private readonly ICouponService couponService;
        private readonly DatabaseContext db;

        public CartAPIController(
            DatabaseContext db, 
            IMapper mapper, 
            IProductService productService, 
            ICouponService couponService
            )
        {
            this.db = db;
            this.mapper = mapper;
            this.productService = productService;
            this.couponService = couponService;
            this.responseDto = new();
        }

        [HttpPost("RemoveCart")]
        public async Task<ResponseDto> RemoveCart([FromBody] long cartDetailsId) 
        {
            try 
            {
                CartDetails cartDetails = db.CartDetails.First(u => u.CartDetailsId == cartDetailsId);

                int totalCountOfCartItems = db.CartDetails.Where(u => u.CartHeaderId == cartDetails.CartHeaderId).Count();

                db.CartDetails.Remove(cartDetails);

                if (totalCountOfCartItems == 1)
                {
                    var cartHeaderToRemove = await db.CartHeaders.FirstOrDefaultAsync(u => u.CartHeaderId == cartDetails.CartHeaderId);

                    db.CartHeaders.Remove(cartHeaderToRemove);

                }

                await db.SaveChangesAsync();

                responseDto.Result = true;
                
            }
            catch(Exception ex) 
            {
                responseDto.Message = ex.Message;
                responseDto.IsSuccessful = false;
            }

            return responseDto;
            
        }

        [HttpPost("CartUpsert")]
        public async Task<ResponseDto> CartUpSert(CartDto cartDto)
        {
            try
            {
                var existingCartHeader = await db.CartHeaders.AsNoTracking().FirstOrDefaultAsync(u => u.UserId == cartDto.CartHeader.UserId);

                if (existingCartHeader == null)
                {
                    CartHeader cartHeader = mapper.Map<CartHeader>(cartDto.CartHeader);
                    db.CartHeaders.Add(cartHeader);
                    await db.SaveChangesAsync();

                    cartDto.CartDetails.First().CartHeaderId = cartHeader.CartHeaderId;
                    db.CartDetails.Add(mapper.Map<CartDetails>(cartDto.CartDetails.First()));
                    await db.SaveChangesAsync();
                }
                else
                {
                    var existingCartDetails = await db.CartDetails.AsNoTracking().FirstOrDefaultAsync(
                        u => u.ProductId == cartDto.CartDetails.First().ProductId &&
                        u.CartHeaderId == existingCartHeader.CartHeaderId);

                    if (existingCartDetails == null)
                    {
                        //Create Cart Details
                        cartDto.CartDetails.First().CartHeaderId = existingCartHeader.CartHeaderId;
                        db.CartDetails.Add(mapper.Map<CartDetails>(cartDto.CartDetails.First()));
                        await db.SaveChangesAsync();

                    }
                    else
                    {
                        //Update Product Quantity
                        cartDto.CartDetails.First().Quantity += existingCartDetails.Quantity;
                        cartDto.CartDetails.First().CartHeaderId = existingCartDetails.CartHeaderId;
                        cartDto.CartDetails.First().CartDetailsId = existingCartDetails.CartDetailsId;
                        db.CartDetails.Update(mapper.Map<CartDetails>(cartDto.CartDetails.First()));
                        await db.SaveChangesAsync();
                    }

                    responseDto.Result = cartDto;
                }
            }
            catch (Exception ex)
            {
                responseDto.Message = ex.Message;
                responseDto.IsSuccessful = false;
            }

            return responseDto;

        }

        [HttpGet("GetCart/{userId}")]
        public async Task<ResponseDto> GetCart(string userId) 
        {
            try 
            {
                CartDto cart = new()
                {
                    CartHeader = mapper.Map<CardHeaderDto>(db.CartHeaders.First(u => u.UserId == userId))
                };

                IEnumerable<ProductDto> productDtos = await productService.GetProductsAsync();

                cart.CartDetails = mapper.Map<IEnumerable<CartDetailsDto>>(db.CartDetails.Where(u => u.CartHeaderId == cart.CartHeader.CartHeaderId));

                foreach(var cartItem in cart.CartDetails) 
                {
                    cartItem.Product = productDtos.FirstOrDefault(u => u.Id == cartItem.ProductId);
                    cart.CartHeader.CartTotal += (cartItem.Quantity * cartItem.Product.Price);
                }

                if (!string.IsNullOrEmpty(cart.CartHeader.CouponCode)) 
                {
                    CouponDto coupon = await couponService.GetCoupon(cart.CartHeader.CouponCode);

                    if(coupon != null) 
                    {
                        var discountedValue = cart.CartHeader.CartTotal - (cart.CartHeader.CartTotal * coupon.DiscountPercentage / 100);
                        cart.CartHeader.CartTotal = discountedValue;
                        cart.CartHeader.Discount = coupon.DiscountPercentage;
                    }
                }

                responseDto.Result = cart;
            }
            catch(Exception ex) 
            {
                responseDto.Message = ex.Message;
                responseDto.IsSuccessful = false;
            }

            return responseDto;
        }

        [HttpPost("ApplyCoupon")]
        public async Task<ResponseDto> ApplyCoupon([FromBody] CartDto cartDto) 
        {
            try 
            {
                var existingCart = await db.CartHeaders.FirstAsync(u => u.UserId == cartDto.CartHeader.UserId);

                var existingCartDetails = await db.CartDetails.FirstAsync(u => u.CartHeaderId == cartDto.CartHeader.CartHeaderId);

                if (string.IsNullOrEmpty(cartDto.CartHeader.CouponCode)) 
                {
                    existingCart.CouponCode = string.Empty;
                    existingCartDetails.CartHeader.CouponCode = string.Empty;
                }

                existingCart.CouponCode = cartDto.CartHeader.CouponCode.ToUpper();
                existingCartDetails.CartHeader.CouponCode = cartDto.CartHeader.CouponCode.ToUpper();

                db.CartHeaders.Update(existingCart);
                await db.SaveChangesAsync();

            }
            catch(Exception ex) 
            {
                responseDto.IsSuccessful=false;
                responseDto.Message = ex.Message;   
            }

            return responseDto;
        }
    }
}

