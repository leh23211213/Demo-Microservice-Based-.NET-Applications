using App.Services.ProductAPI.Models;
using App.Services.ProductAPI.Models.DTOs;
using AutoMapper;

namespace App.Services.ProductAPI
{
    public class MappingConfig : Profile
    {
        public MappingConfig()
        {
            CreateMap<ProductDTO, Product>().ReverseMap();
            CreateMap<PaginationDTO, Pagination>().ReverseMap();
        }
    }
}
