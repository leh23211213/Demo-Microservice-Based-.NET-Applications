using App.Services.ProductAPI.Models;
using App.Services.ProductAPI.Models.DTOs;
using AutoMapper;

namespace Mango.Services.ProductAPI
{
    public class MappingConfig
    {
        public static MapperConfiguration RegisterMaps()
        {
            var mappingConfig = new MapperConfiguration(config =>
            {
                config.CreateMap<ProductDTO, Product>().ReverseMap();
                config.CreateMap<SizeDTO, Size>().ReverseMap();
                config.CreateMap<CategoryDTO, Category>().ReverseMap();
                config.CreateMap<ColorDTO, Color>().ReverseMap();
                config.CreateMap<BrandDTO, Brand>().ReverseMap();
            });
            return mappingConfig;
        }
    }
}
