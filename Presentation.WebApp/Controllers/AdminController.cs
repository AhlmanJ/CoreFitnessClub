using Application.Common.Roles;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Presentation.WebApp.ViewModels;

namespace Presentation.WebApp.Controllers
{
    [Authorize(Roles = ApplicationRoles.Admin)]
    [Route("admin-page")]
    public class AdminController : Controller
    {
        public IActionResult Index(string section = "members")
        {
            var vm = new AdminViewModel
            {
                Section = section
            };

            return View();
        }
    }
}
