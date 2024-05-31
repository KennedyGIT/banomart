using AutoMapper;
using Banomart.Services.CouponAPI.Models;
using Banomart.Services.CouponAPI.Models.DTOs;

namespace Banomart.Services.CouponAPI
{
    public class MappingProfiles
    {
        public static MapperConfiguration RegisterMappings() 
        {
            var mappingConfig = new MapperConfiguration(config => 
            {
                config.CreateMap<CouponDto, Coupon>();
                config.CreateMap<Coupon, CouponDto>();
            });

            return mappingConfig;
        }
    }
}
