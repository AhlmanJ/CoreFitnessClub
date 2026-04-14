using Application.Common.Roles;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.WebApp.Controllers;

[Authorize(Roles = ApplicationRoles.Admin)]
[Route("Membership-plans")]
public class MembershipPlansController : Controller
{
    public IActionResult Index()
    {
        
    }

    public IActionResult Create()
    {

    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Create() 
    { 
    
    }

    public IActionResult Edit(Guid id)
    {

    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Edit(EditMembershipPlanViewModel)
    {

    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Delete(Guid id)
    {

    }
}