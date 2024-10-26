
using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using template.Data;
using template.Models;
namespace template.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/product")]
    public class ReadProductAPIController : Controller
    {
        [HttpGet]
        public ActionResult<List<int>> GetProduct(){
            return new List<int> { 1, 2, 3 };
        }
    }
}
