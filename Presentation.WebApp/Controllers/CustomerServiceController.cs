using Microsoft.AspNetCore.Mvc;
using Presentation.WebApp.ViewModels.CustomerService;

namespace Presentation.WebApp.Controllers
{
    public class CustomerServiceController : Controller
    {
        public IActionResult Index()
        {
            ViewData["Title"] = "CustomerService";

            return View();
        }

        [HttpPost]
        public IActionResult Contact(ContactFormViewModel model)
        {
            if (!ModelState.IsValid)
                return View("Index",model);


            return RedirectToAction("Index");
        }
    }
}
