using App.Services.ProductAPI.Models;
using AutoMapper;

namespace App.Services.ProductAPI
{
    public class MappingConfig
    {
        public static MapperConfiguration RegisterMaps()
        {
            var mappingConfig = new MapperConfiguration(config =>
            {
                config.CreateMap<Product, ProductDto>().ReverseMap();

                config.CreateMap<Product, ProductDetailsDto>().ReverseMap();
                config.CreateMap<Product, PaginateProduct>().ReverseMap();
            });
            return mappingConfig;
        }
    }
}
