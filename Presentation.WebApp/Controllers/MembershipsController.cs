using Microsoft.AspNetCore.Mvc;

namespace Presentation.WebApp.Controllers
{
    public class MembershipsController : Controller
    {
        public IActionResult Index()
        {
            ViewData["Title"] = "Memberships";

            return View();
        }
    }
}
