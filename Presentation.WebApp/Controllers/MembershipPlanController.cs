
/*
 * I have taken help from both the lectures at school and chatGPT to be able to create this controller.
 * I have recived a lot of guidance from AI to be able to understand how to implement MVC, services and input/outputs as it was very difficult to understand.
 */ 

using Application.Abstraction.MembershipPlansInterface;
using Application.Common.Roles;
using Application.MembershipPlans.Inputs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Presentation.WebApp.Models.MembershipPlanModels;
using Presentation.WebApp.ViewModels;

namespace Presentation.WebApp.Controllers;

[Authorize(Roles = ApplicationRoles.Admin)]
[Route("MembershipPlan")]
public class MembershipPlanController(ICreateMembershipPlanService createMembershipPlanService, IGetAllMembershipPlansService getAllMembershipPlansService, IDeleteMembershipPlanService deleteMembershipPlanService, IGetMembershipPlanService getMembershipPlanService, IUpdateMembershipPlanService updateMembershipPlanService) : Controller
{
    [HttpGet("Index")]
    public async Task<IActionResult> Index()
    {
        var result = await getAllMembershipPlansService.ExecuteAsync();

        if (!result.Success || result.Value == null)
        {
            ViewData["ErrorMessage"] = result.ErrorMessage;
            return View( new MembershipPlanViewModel { MembershipPlans = new List<MembershipPlan>()});
        }

        var viewModel = new MembershipPlanViewModel
        {
            MembershipPlans = result.Value.Select(plan => new MembershipPlan
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

    [HttpGet("Create")]
    public IActionResult Create()
    {
        return View();
    }

    [HttpPost("Create")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(MembershipPlanViewModel viewModel) 
    {
        if (!ModelState.IsValid)
            return View("index",viewModel);

        var input = new CreateMembershipPlanInput
            (
                viewModel.Form.Name,
                viewModel.Form.Description,
                viewModel.Form.Price,
                viewModel.Form.ValidDays
            );

       var result = await createMembershipPlanService.ExecuteAsync(input);

        if (!result.Success)
        {
            ViewData["ErrorMessage"] = result.ErrorMessage;
            return View("Index",viewModel);
        }

        return RedirectToAction("Index",viewModel);
    }

    [HttpGet("Update/{id}")]
    public async Task<IActionResult> Update(Guid id)
    {
        var result = await getMembershipPlanService.ExecuteAsync(id);
        if (!result.Success)
        {
            ViewData["ErrorMessage"] = "Could not find the membership plan";
            return View("Index");
        }

        var plan = result.Value;
        if(plan is null)
        {
            ViewData["ErrorMessage"] = "The requested membership plan does not exist";
            return View("Index");
        }

        var viewModel = new MembershipPlanViewModel
        {
            Id = plan.Id,
            Form = new MembershipPlanForm
            {
                Name = plan.Name,
                Description = plan.Description,
                Price = plan.Price,
                ValidDays = plan.ValidDays
            }
        };

        return View("Index",viewModel);
    }

    [HttpPost("Update/{id}")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Update(MembershipPlanViewModel viewModel)
    {
        if (!ModelState.IsValid)
            return View("Index", viewModel);

        var input = new UpdateMembershipPlanInput
            (
                viewModel.Id,
                viewModel.Form.Name,
                viewModel.Form.Description,
                viewModel.Form.Price,
                viewModel.Form.ValidDays
            );

        var result = await updateMembershipPlanService.ExecuteAsync(input);

        if (!result.Success)
        {
            ViewData["ErrorMessage"] =result.ErrorMessage;
            return View("Index", viewModel);
        }

        return RedirectToAction("Index",viewModel);
    }

    [HttpPost("Delete/{id}")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(Guid id)
    {
        if (id == Guid.Empty)
            return BadRequest("Invalid ID.");

        await deleteMembershipPlanService.ExecuteAsync(id);
        return RedirectToAction("Index");
    }
}