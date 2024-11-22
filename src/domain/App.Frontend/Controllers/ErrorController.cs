using App.Frontend.Models;
using Microsoft.AspNetCore.Mvc;

public class ErrorController : Controller
{
    [Route("Error/RequestTimeout")]
    public IActionResult RequestTimeout()
    {
        return View(new ErrorModel() { Message = "Your request took too long to process. Please try again later.", });
    }
}
