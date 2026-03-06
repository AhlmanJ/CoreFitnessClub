using Microsoft.AspNetCore.Mvc;

namespace Presentation.WebApp.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Home()
        {
            ViewData["Title"] = "Home";

            return View();
        }
    }
}
