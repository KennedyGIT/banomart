using AutoMapper;
using Banomart.Services.ProductAPI.Models;
using Banomart.Services.ProductAPI.Models.DTOs;

namespace Banomart.Services.ProductAPI
{
    public class MappingProfiles
    {
        public static MapperConfiguration RegisterMappings() 
        {
            var mappingConfig = new MapperConfiguration(config => 
            {
                config.CreateMap<ProductDto, Product>();
                config.CreateMap<Product, ProductDto>();
            });

            return mappingConfig;
        }
    }
}
