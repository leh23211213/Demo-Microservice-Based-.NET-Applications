using App.Services.ProductAPI.Models;
using App.Services.ProductAPI.Models.DTOs;
using AutoMapper;

namespace App.Services.ProductAPI
{
    public class MappingConfig
    {
        public static MapperConfiguration RegisterMaps()
        {
            var mappingConfig = new MapperConfiguration(config =>
            {
                config.CreateMap<ProductDTO, Product>()
                    .ForMember(dest => dest.Size, opt => opt.Ignore())
                    .ForMember(dest => dest.Color, opt => opt.Ignore())
                    .ForMember(dest => dest.Category, opt => opt.Ignore())
                    .ForMember(dest => dest.Brand, opt => opt.Ignore())
                    .ReverseMap();
                config.CreateMap<SizeDTO, Size>().ReverseMap();
                config.CreateMap<CategoryDTO, Category>().ReverseMap();
                config.CreateMap<ColorDTO, Color>().ReverseMap();
                config.CreateMap<BrandDTO, Brand>().ReverseMap();
            });
            return mappingConfig;
        }
    }
}
