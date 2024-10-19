using App.Services.OrderAPI.Models;
using AutoMapper;

namespace App.Services.OrderAPI
{
    public class MappingConfig
    {
        public static MapperConfiguration RegisterMaps()
        {
            var mappingConfig = new MapperConfiguration(config =>
            {
                config.CreateMap<OrderHeader, CartHeader>()
                .ForMember(dest => dest.Total, u => u.MapFrom(src => src.OrderTotal)).ReverseMap();

                config.CreateMap<CartDetails, OrderDetails>()
                .ForMember(dest => dest.ProductName, u => u.MapFrom(src => src.Product.Name))
                .ForMember(dest => dest.Price, u => u.MapFrom(src => src.Product.Price))
                .ReverseMap();
            });
            return mappingConfig;
        }
    }
}
