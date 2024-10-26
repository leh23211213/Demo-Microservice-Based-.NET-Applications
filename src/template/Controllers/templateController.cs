using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace template.Controllers
{
    [Route("[controller]")]
    public class templateController : Controller
    {

        [HttpGet]
        public ActionResult<List<int>> Get()
        {
            return new List<int> { 1, 2, 3 };
        }
    }
}