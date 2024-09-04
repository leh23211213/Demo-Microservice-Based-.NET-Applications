
using App.Services.AuthAPI.Data;
using App.Services.ProductAPI.Models.DTOs;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace App.Services.ProductAPI.Controllers
{
    [ApiController]
    [Route("api/product")]
    public class ProductAPIController : ControllerBase
    {
        private readonly ApplicationDbContext _dbContext;
        private Response _response;
        private IMapper _mapper;

        public ProductAPIController(ApplicationDbContext dbContext, Response response, IMapper mapper)
        {
            _dbContext = dbContext;
            _response = new Response();
            _mapper = mapper;
        }

        

    }
}