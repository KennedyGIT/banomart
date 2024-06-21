using AutoMapper;
using bano_mart_mvc.Models;
using bano_mart_mvc.Service;
using bano_mart_mvc.Service.IService;
using Banomart.Services.ProductAPI.Data;
using IdentityModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Diagnostics;

namespace bano_mart_mvc.Controllers
{
    public class HomeController : Controller
    {

        private readonly IProductService productService;
        private readonly ICartService cartService;

        public HomeController(IProductService productService, ICartService cartService)
        {
            this.productService = productService;
            this.cartService = cartService;
        }

        public async Task<IActionResult> Index()
        {
            List<ProductDto>? list = new();

            ResponseDto? response = await productService.GetAllProductsAsync();

            if (response != null && response.IsSuccessful)
            {
                list = JsonConvert.DeserializeObject<List<ProductDto>>(Convert.ToString(response.Result));
            }
            else
            {
                TempData["error"] = response?.Message;
            }

            return View(list);
        }


        [Authorize]
        public async Task<IActionResult> Details(int productId)
        {
            ProductDto? product = new();

            ResponseDto? response = await productService.GetProdyctByIdAsync(productId);

            if (response != null && response.IsSuccessful)
            {
                product = JsonConvert.DeserializeObject<ProductDto>(Convert.ToString(response?.Result));
            }
            else
            {
                TempData["error"] = response?.Message;
            }

            return View(product);
        }


        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Details(ProductDto productDto)
        {
            CartDto cartDto = new CartDto();

            cartDto.CartHeader = new CardHeaderDto()
            {
                UserId = User.Claims.Where(u => u.Type == JwtClaimTypes.Subject)?.FirstOrDefault()?.Value
            };

            CartDetailsDto cartDetailsDto = new CartDetailsDto()
            {
                Quantity = productDto.Quantity,
                ProductId = (int)productDto.Id
            };

            List<CartDetailsDto> cartDetailsDtos = new() { cartDetailsDto };

            cartDto.CartDetails = cartDetailsDtos;

            var response = await cartService.UpsertCartAsync(cartDto);

            if (response != null && response.IsSuccessful)
            {
                TempData["success"] = "Added to cart successfully";
                return RedirectToAction(nameof(Index));
            }
            else
            {
                TempData["error"] = response?.Message;
            }

            return View(productDto);
        }

    }
}
