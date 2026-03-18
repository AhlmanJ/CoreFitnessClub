using Microsoft.AspNetCore.Mvc;

namespace Presentation.WebApp.Controllers
{
    public class CustomerServiceController : Controller
    {
        public IActionResult Index()
        {
            ViewData["Title"] = "CustomerService";

            return View();
        }
    }
}
