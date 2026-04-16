using Application.Abstraction.MembershipPlansInterface;
using Presentation.WebApp.Models.MembershipPlanModels;
using Microsoft.AspNetCore.Mvc;
using Presentation.WebApp.ViewModels;

namespace Presentation.WebApp.Controllers;

public class MembershipsController : Controller
{
    private readonly IGetAllMembershipPlansService _getAllMembershipPlansService;

    public MembershipsController(IGetAllMembershipPlansService getAllMembershipPlansService)
    {
        _getAllMembershipPlansService = getAllMembershipPlansService;
    }

    public async Task<IActionResult> Index()
    {
        ViewData["Title"] = "Memberships";

        var result = await _getAllMembershipPlansService.ExecuteAsync();

        if (!result.Success)
        {
            ViewData["ErrorMessage"] = result.ErrorMessage;
            return View();
        }

        var viewModel = new MembershipPlanViewModel
        {
            MembershipPlans = result.Value!.Select(plan => new MembershipPlan
            {
                Id = plan.Id,
                Name = plan.Name,
                Description = plan.Description,
                Price = plan.Price,
                ValidDays = plan.ValidDays

            }).ToList() ?? new List<MembershipPlan>()
        };

        return View(viewModel);
    }
}
