using Microsoft.AspNetCore.Mvc;

namespace Presentation.WebApp.Controllers
{
    public class ErrorController : Controller
    {
        [Route("Error/{statusCode}")]
        public IActionResult ErrorHandler(int statusCode)
        {
            return statusCode switch
            {
                404 => View("NotFound"),
                _ => View("NotFound")
            };
        }
    }
}
