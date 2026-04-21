
// I wasn't shure how to design this page, but i decided that the best way was to design it with different controllers and separate pages.

using Application.Common.Roles;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace Presentation.WebApp.Controllers
{
    [Authorize(Roles = ApplicationRoles.Admin)]
    [Route("admin-page")]
    public class AdminController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
