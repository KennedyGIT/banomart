using AutoMapper;
using Banomart.Services.ShoppingCartAPI.Models;
using Banomart.Services.ShoppingCartAPI.Models.Dto;

namespace Banomart.Services.ShoppingCartAPI
{
    public class MappingProfiles
    {
        public static MapperConfiguration RegisterMappings() 
        {
            var mappingConfig = new MapperConfiguration(config => 
            {
                config.CreateMap<CartHeader, CardHeaderDto>().ReverseMap();
                config.CreateMap<CartDetails, CartDetailsDto>().ReverseMap();
            });

            return mappingConfig;
        }
    }
}
