using bano_mart_mvc.Models;
using bano_mart_mvc.Service.IService;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace bano_mart_mvc.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductService productService;

        public ProductController(IProductService productService)
        {
            this.productService = productService;
        }



        
        public async Task<IActionResult> ProductIndex()
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

        public async Task<IActionResult> ProductCreate() 
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ProductCreate(ProductDto productDto)
        {
            if(ModelState.IsValid) 
            {
                ResponseDto? response = await productService.CreateProductAsync(productDto);

                if (response != null && response.IsSuccessful)
                {
                    TempData["success"] = $"Product Created Successfully";
                    return RedirectToAction(nameof(ProductIndex));
                }
                else 
                {
                    TempData["error"] = response?.Message;
                }
            }

            return View();
        }

		public async Task<IActionResult> ProductEdit(long productId)
		{
			ProductDto product = new();

			ResponseDto? response = await productService.GetProdyctByIdAsync((int)productId);

			if (response != null && response.IsSuccessful)
			{
				product = JsonConvert.DeserializeObject<ProductDto>(Convert.ToString(response.Result));

				return View(product);
			}

			return NotFound();
		}

		[HttpPost]
		public async Task<IActionResult> ProductEdit(ProductDto productDto)
		{
			CouponDto coupon = new();

			ResponseDto? response = await productService.UpdateProductAsync(productDto);

			if (response != null && response.IsSuccessful)
			{
				coupon = JsonConvert.DeserializeObject<CouponDto>(Convert.ToString(response.Result));

				TempData["success"] = "Product Updated Successfully";

				return RedirectToAction(nameof(ProductIndex));
			}
			else
			{
				TempData["error"] = response?.Message;
			}

			return View(productDto);
		}


		public async Task<IActionResult> ProductDelete(long productId)
        {
            ProductDto product = new();

            ResponseDto? response = await productService.GetProdyctByIdAsync((int)productId);

            if (response != null && response.IsSuccessful)
            {
                product = JsonConvert.DeserializeObject<ProductDto>(Convert.ToString(response.Result));

                return View(product);
            }

            return NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> ProductDelete(ProductDto productDto)
        {
            CouponDto coupon = new();

            ResponseDto? response = await productService.DeleteProductAsync((int)productDto.Id);

            if (response != null && response.IsSuccessful)
            {
                coupon = JsonConvert.DeserializeObject<CouponDto>(Convert.ToString(response.Result));

                TempData["success"] = "Product Deleted Successfully";

                return RedirectToAction(nameof(ProductIndex));
            }
            else
            {
                TempData["error"] = response?.Message;
            }

            return View(productDto);
        }



    }
}
