
/*
 * I have had a very hard time inderstanding when to use return View() and when to use ReturnToAction(),
 * also Routing and how to combine different ViewModels when i want to send to other controllers and partials,
 * and to create HttpGet("Index"), HttpPost("Enroll/{id}"), i have recived ALOT of support from chatGPT and tested alot of code in different ways. 
 */


using Application.Abstraction.MembershipInterface;
using Application.Abstraction.MembershipPlansInterface;
using Application.Memberships.Inputs;
using Infrastructure.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Presentation.WebApp.ViewModels;


namespace Presentation.WebApp.Controllers;

[Route("Membership")]
public class MembershipsController(IGetAllMembershipPlansService getAllMembershipPlansService,IEnrollMembershipService enrollMembershipService ,IDeleteMembershipByMemberIdService deleteMembershipByMemberIdService , UserManager<ApplicationUser> userManager) : Controller
{

    // ----------- Help from chatGPT -----------------

    [HttpGet("Index")]
    public async Task<IActionResult> Index()
    {
        ViewData["Title"] = "Memberships";

        var result = await getAllMembershipPlansService.ExecuteAsync();

        if (!result.Success)
        {
            ViewData["ErrorMessage"] = result.ErrorMessage;
            {
                ViewData["ErrorMessage"] = result.ErrorMessage;
                return View(new MembershipPlanViewModel { Plans = new List<MembershipPlanListViewModel>() });
            }
        }

        var viewModel = new MembershipPlanViewModel
        {
            Plans = result.Value!.Select(plan => new MembershipPlanListViewModel
            {
                Id = plan.Id,
                Name = plan.Name,
                Description = plan.Description,
                ListItem1 = plan.ListItem1,
                ListItem2 = plan.ListItem2,
                ListItem3 = plan.ListItem3,
                Price = plan.Price,
                ValidDays = plan.ValidDays

            }).ToList() ?? new List<MembershipPlanListViewModel>()
        };

        return View(viewModel);
    }

    [HttpGet("Enroll")]
    public async Task<IActionResult> Enroll()
    {
        return View();
    }

    [Authorize]
    [HttpPost("Enroll/{id}")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Enroll(Guid id)
    {
        if(!ModelState.IsValid)
            return View();

        var user = await userManager.GetUserAsync(User);
        if (user is null)
        {
            TempData["ErrorMessage"] = "The requested User could not be found."; // TempData because ViewData and ViewBag can't bring data to another Http-request. 
            return RedirectToAction("Index");
        }

        var input = new EnrollMembershipInput
            (
                user.Id,
                id
            );

        var result = await enrollMembershipService.ExecuteAsync(input);
        if (!result.Success)
        {
            TempData["ErrorMessage"] = result.ErrorMessage;
            return RedirectToAction("Index");
        }

        var output = result.Value;
        var membershipVm = new MembershipViewModel // Map to ViewModel
        { 
            FirstName = output!.FirstName,
            LastName = output.LastName,
            Name = output.Name,
            Price = output.Price,
            StartDate = output.StartDate,
            EndDate = output.EndDate
        };

        return RedirectToAction("My", "Account"); // Redirect to action with route values (query parameter)
    }

    // ----------- Help from chatGPT - END -----------------

    [HttpPost("DeleteMembership/{memberId}")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteMembership(Guid memberId)
    {
        if (memberId == Guid.Empty)
            return BadRequest("Invalid ID.");

        var result = await deleteMembershipByMemberIdService.ExecuteAsync(memberId);
        return RedirectToAction("My","Account");
    }
}