using AutoMapper;
using bano_mart_mvc.Models;
using bano_mart_mvc.Service;
using bano_mart_mvc.Service.IService;
using Banomart.Services.ProductAPI.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Diagnostics;

namespace bano_mart_mvc.Controllers
{
    public class HomeController : Controller
    {

        private readonly IProductService productService;

        public HomeController(IProductService productService)
        {
            this.productService = productService;
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


    }
}
