using Application.Abstraction.MembershipInterface;
using Application.Common.Roles;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Presentation.WebApp.ViewModels;

namespace Presentation.WebApp.Controllers;

[Authorize(Roles = ApplicationRoles.Admin)]
[Route("ManageMemberships")]
public class ManageMemberships(IGetAllMembershipsService getAllMembershipsService, IDeleteMembershipByMemberIdService deleteMembershipByMemberIdService) : Controller
{
    [HttpGet("Index")]
    public async Task<IActionResult> Index()
    {
        var result = await getAllMembershipsService.ExecuteAsync();

        if (!result.Success || result.Value == null)
        {
            ViewData["ErrorMessage"] = result.ErrorMessage;
            return View(new MembershipListViewModel());
        }

        var viewModel = new MembershipViewModel
        { 
            Memberships = result.Value.Select(memberships => new MembershipListViewModel 
            { 
                MemberId = memberships.memberId,
                FirstName = memberships.FirstName,
                LastName = memberships.LastName,
                Name = memberships.Name,
                Price = memberships.Price,
                StartDate = memberships.StartDate,
                EndDate = memberships.EndDate

            }).ToList()
        };

        return View(viewModel);
    }

    [HttpPost("DeleteMembership/{memberId}")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteMembership(Guid memberId)
    {
        if (memberId == Guid.Empty)
            return BadRequest("Invalid ID.");

        var result = await deleteMembershipByMemberIdService.ExecuteAsync(memberId);
        return RedirectToAction("Index", "Admin");
    }
}