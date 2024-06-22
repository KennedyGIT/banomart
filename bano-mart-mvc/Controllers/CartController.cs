using bano_mart_mvc.Models;
using bano_mart_mvc.Service.IService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;

namespace bano_mart_mvc.Controllers
{
    public class CartController : Controller
    {
        private readonly ICartService cartService;

        public CartController(ICartService cartService)
        {
            this.cartService = cartService;
        }


        [Authorize]
        public async Task<IActionResult> CartIndex()
        {
            CartDto cartDto = await GetCartByUserId();

            if (cartDto != null)
            {
                return View(cartDto);
            }
            else
            {
                var emptyCartDto = new CartDto();
                emptyCartDto.CartDetails = new List<CartDetailsDto>();
                emptyCartDto.CartHeader = new CardHeaderDto();
                return View(emptyCartDto);
            } 
        }

        public async Task<IActionResult> Remove(int CartDetailsId) 
        {

            ResponseDto responseDto = await cartService.RemoveFromCartAsync(CartDetailsId);

            if ((responseDto != null) && responseDto.IsSuccessful)
            {
                TempData["success"] = "Cart updated successfully";
                return RedirectToAction(nameof(CartIndex));
            }
            else
            {
                return View();
            }
        }

        [HttpPost]
        public async Task<IActionResult> ApplyCoupon(CartDto cartDto)
        {
            cartDto.CartDetails = new List<CartDetailsDto>();

            ResponseDto responseDto = await cartService.ApplyCouponAsync(cartDto);

            if ((responseDto != null) && responseDto.IsSuccessful)
            {
                TempData["success"] = "Cart updated successfully";
                return RedirectToAction(nameof(CartIndex));
            }
            else
            {
                return View();
            }
        }

        [HttpPost]
        public async Task<IActionResult> EmailCart()
        {
            CartDto cartDto = await GetCartByUserId();

            if(cartDto != null)
            {

                ResponseDto responseDto = await cartService.EmailCart(cartDto);

                TempData["success"] = "Details of your cart has been emailed to you";
                
            }
            else 
            {
                return RedirectToAction(nameof(CartIndex));
            }

            return RedirectToAction(nameof(CartIndex));
        }

        [HttpPost]
        public async Task<IActionResult> RemoveCoupon(CartDto cartDto)
        {
            cartDto.CartDetails = new List<CartDetailsDto>();

            cartDto.CartHeader.CouponCode = string.Empty;

            ResponseDto responseDto = await cartService.ApplyCouponAsync(cartDto);

            if ((responseDto != null) && responseDto.IsSuccessful)
            {
                TempData["success"] = "Cart updated successfully";
                return RedirectToAction(nameof(CartIndex));
            }
            else
            {
                return View();
            }
        }

        private async Task<CartDto> GetCartByUserId() 
        {
            var userId = User.Claims.Where(u => u.Type == JwtRegisteredClaimNames.Sub)?.FirstOrDefault()?.Value;

            ResponseDto responseDto = await cartService.GetCartByUserIdAsync(userId);

            if ((responseDto != null) && (responseDto.IsSuccessful) && (responseDto.Result != null))
            {
                CartDto cartDto = JsonConvert.DeserializeObject<CartDto>(Convert.ToString(responseDto.Result));

                cartDto.CartHeader.Email = User.Claims.Where(u => u.Type == JwtRegisteredClaimNames.Email)?.FirstOrDefault()?.Value;

                return cartDto;
            }
            else
            {
                return null;
            }
        }
    }
}
