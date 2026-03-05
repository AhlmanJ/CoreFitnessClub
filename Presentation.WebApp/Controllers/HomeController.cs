using Microsoft.AspNetCore.Mvc;

namespace Presentation.WebApp.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Home()
        {
            ViewData["Tilte"] = "Home";

            return View();
        }
    }
}
