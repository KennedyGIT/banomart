using bano_mart_mvc.Models;
using bano_mart_mvc.Service.IService;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace bano_mart_mvc.Controllers
{
    public class CouponController : Controller
    {
        private readonly ICouponService couponService;

        public CouponController(ICouponService couponService)
        {
            this.couponService = couponService;
        }


        public async Task<IActionResult> CouponIndex()
        {
            List<CouponDto>? list = new();

            ResponseDto? response = await couponService.GetAllCouponAsync();

            if(response != null && response.IsSuccessful) 
            {
                list = JsonConvert.DeserializeObject<List<CouponDto>>(Convert.ToString(response.Result));
            }

            return View(list);
        }

        public async Task<IActionResult> CouponCreate() 
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CouponCreate(CouponDto couponDto)
        {
            if(ModelState.IsValid) 
            {
                ResponseDto? response = await couponService.CreateCouponsAsync(couponDto);

                if (response != null && response.IsSuccessful)
                {
                    TempData["success"] = $"Coupon Created Successfully";
                    return RedirectToAction(nameof(CouponIndex));
                }
                else 
                {
                    TempData["error"] = response?.Message;
                }
            }

            return View();
        }

        public async Task<IActionResult> CouponDelete(long couponId)
        {
            CouponDto coupon = new();

            ResponseDto? response = await couponService.GetCouponByIdAsync((int)couponId);

            if (response != null && response.IsSuccessful)
            {
                coupon = JsonConvert.DeserializeObject<CouponDto>(Convert.ToString(response.Result));

                return View(coupon);
            }

            return NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> CouponDelete(CouponDto couponDto)
        {
            CouponDto coupon = new();

            ResponseDto? response = await couponService.DeleteCouponsAsync((int)couponDto.Id);

            if (response != null && response.IsSuccessful)
            {
                coupon = JsonConvert.DeserializeObject<CouponDto>(Convert.ToString(response.Result));

                TempData["success"] = $"Coupon Deleted Successfully";

                return RedirectToAction(nameof(CouponIndex));
            }
            else
            {
                TempData["error"] = response?.Message;
            }

            return View(couponDto);
        }



    }
}
