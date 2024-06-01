using AutoMapper;
using Banomart.Services.CouponAPI.Data;
using Banomart.Services.CouponAPI.Models;
using Banomart.Services.CouponAPI.Models.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace Banomart.Services.CouponAPI.Controllers
{
    [Route("api/coupon")]
    [ApiController]
    public class CouponController : ControllerBase
    {
        private readonly DatabaseContext db;
        private readonly IMapper mapper;
        private ResponseDto responseDto;

        public CouponController(DatabaseContext db, IMapper mapper) 
        {
            this.db = db;
            this.mapper = mapper;
            responseDto = new ResponseDto();
        }


        [HttpGet]
        public ResponseDto Get() 
        {
            try 
            {
                IEnumerable<Coupon> coupons = db.Coupons.ToList();
                responseDto.Result = this.mapper.Map<IEnumerable<CouponDto>>(coupons);
            }
            catch (Exception ex) 
            {
                responseDto.IsSuccessful = false;
                responseDto.Message = ex.Message;
            }

            return responseDto;
        }


        [HttpGet]
        [Route("{id:int}")]
        public ResponseDto Get(int id)
        {
            try
            {
                Coupon coupon = db.Coupons.First(c => c.Id == id);
                responseDto.Result = this.mapper.Map<CouponDto>(coupon);
            }
            catch (Exception ex)
            {
                responseDto.IsSuccessful = false;
                responseDto.Message = ex.Message;
            }

            return responseDto;
        }


        [HttpGet]
        [Route("GetByCode/{code}")]
        public ResponseDto GetByCode(string code)
        {
            try
            {
                Coupon coupon = db.Coupons.FirstOrDefault(c => c.CouponCode.ToLower() == code.ToLower());

                if (coupon == null) 
                {
                    responseDto.IsSuccessful = false;
                }
                responseDto.Result = this.mapper.Map<CouponDto>(coupon);
            }
            catch (Exception ex)
            {
                responseDto.IsSuccessful = false;
                responseDto.Message = ex.Message;
            }

            return responseDto;
        }

        [HttpPost]
        public ResponseDto Post([FromBody] CouponDto couponDto)
        {
            try
            {
                Coupon coupon = mapper.Map<Coupon>(couponDto);
                db.Coupons.Add(coupon);
                db.SaveChanges();

                responseDto.Result = this.mapper.Map<CouponDto>(coupon);
            }
            catch (Exception ex)
            {
                responseDto.IsSuccessful = false;
                responseDto.Message = ex.Message;
            }

            return responseDto;
        }

        [HttpPut]
        public ResponseDto Put([FromBody] CouponDto couponDto)
        {
            try
            {
                Coupon coupon = mapper.Map<Coupon>(couponDto);
                db.Coupons.Update(coupon);
                db.SaveChanges();

                responseDto.Result = this.mapper.Map<CouponDto>(coupon);
            }
            catch (Exception ex)
            {
                responseDto.IsSuccessful = false;
                responseDto.Message = ex.Message;
            }

            return responseDto;
        }

        [HttpDelete]
        [Route("{id:int}")]
        public ResponseDto Delete(long id)
        {
            try
            {
                Coupon coupon = db.Coupons.First(c => c.Id == id);
                db.Coupons.Remove(coupon);
                db.SaveChanges();

                responseDto.Result = this.mapper.Map<CouponDto>(coupon);
            }
            catch (Exception ex)
            {
                responseDto.IsSuccessful = false;
                responseDto.Message = ex.Message;
            }

            return responseDto;
        }
    }
}
