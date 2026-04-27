
/* 
 * ------------! Help by chatGPT to solve a problem !----------------
 * 
 *  I had a problem where the member list was empty if there was a validation error in the form that caused the page to be re-rendered.
 *  This was because when the page was rendered for the second time thorugh a nother IAction-method, the members were not loaded from the "memberQueryService" -
 *  - because the method was only applied in the index-Action.
 *  
 *  Therefore, i asked chatGPT how i could solve this problem. The solution was to created a "PopulateMembersAsync()" method - 
 *  - to always load the members to be used in the dropdown menue when the page was re-rendered.
 *  
 */


using Application.Abstraction.MembersQueryInterface;
using Application.Abstraction.TrainingSessionQueryInterface;
using Application.Abstraction.TrainingSessionsInterface;
using Application.Common.Roles;
using Application.TrainingSessions.Inputs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Presentation.WebApp.Models.TrainingSessionModels;
using Presentation.WebApp.ViewModels;

namespace Presentation.WebApp.Controllers;

[Authorize(Roles = ApplicationRoles.Admin)]
[Route("TrainingSessionAdmin")]
public class TrainingSessionAdminController(IMemberQueryService memberQueryService, ICreateTrainingSessionService createTrainingSessionService, ITrainingSessionQueryService trainingSessionQueryService, IGetAllTrainingSessionsService getAllTrainingSessionsService, IUpdateTrainingSessionService updateTrainingSessionService,IDeleteTrainingSessionService deleteTrainingSessionService) : Controller
{
    [HttpGet("Index")]
    public async Task<IActionResult> Index()
    {
        var result = await getAllTrainingSessionsService.ExecuteAsync();
        var members = await memberQueryService.GetAllMembersAsync();

        if (!result.Success)
        {
            ViewData["ErrorMessage"] = result.ErrorMessage;
            {
                ViewData["ErrorMessage"] = result.ErrorMessage;
                return View(new TrainingSessionViewModel { Sessions = new List<TrainingSessionListViewModel>() });
            }
        }

        var viewModel = new TrainingSessionViewModel
        {
            Sessions = result.Value!.Select(session => new TrainingSessionListViewModel
            {
                Id = session.Id,
                SessionName = session.SessionName,
                TrainerFirstName = session.TrainerFirstName,
                TrainerLastName = session.TrainerLastName,
                CreatedAt = session.CreatedAt,
                StartDate = session.StartDate,
                EndDate = session.EndDate,
                Capacity = session.Capacity,
                Location = session.Location

            }).ToList(),

            Members = members.Select(members => new MemberListViewModel
            {
                Id = members.Id,
                UserId = members.UserId,
                FirstName = members.FirstName,
                LastName = members.LastName,
                PhoneNumber = members.PhoneNumber,
                ProfileImageUrl = members.ProfileImageUrl,


            }).ToList()
        };

        return View(viewModel);
    }

    [HttpGet("Create")]
    public IActionResult Create()
    {
        return View(new TrainingSessionViewModel());
    }

    [HttpPost("Create")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(TrainingSessionViewModel viewModel)
    {
        if (!ModelState.IsValid)
        {
            await PopulateMembersAsync(viewModel);
            return View("Index", viewModel);
        }

        var input = new CreateTrainingSessionInput
            (
                viewModel.Form.TrainerMemberId,
                viewModel.Form.SessionName,
                viewModel.Form.StartDate,
                viewModel.Form.EndDate,
                viewModel.Form.Capacity,
                viewModel.Form.Location!
            );

        var result = await createTrainingSessionService.ExecuteAsync(input);

        if(!result.Success)
        {
            TempData["ErrorMessage"] = result.ErrorMessage;
            return View("Index", viewModel);
        }

        return RedirectToAction("Index");
    }

    [HttpGet("update/{id}")]
    public async Task<IActionResult> Update(Guid id) 
    { 
        var result = await trainingSessionQueryService.GetTrainingSessionByIdAsync(id);
        if (result is null)
        {
            TempData["ErrorMessage"] = "Could not find the training session";
            return View("Index");
        }

        var members = await memberQueryService.GetAllMembersAsync();

        var viewModel = new TrainingSessionViewModel
        {
            Form = new TrainingSessionForm
            {
                TrainerMemberId = result.TrainerMemberId,
                SessionName = result.SessionName,
                StartDate = result.StartDate,
                EndDate = result.EndDate,
                Capacity = result.Capacity,
                Location = result.Location
            },

            Members = members.Select(members => new MemberListViewModel
            {
                Id = members.Id,
                UserId = members.UserId,
                FirstName = members.FirstName,
                LastName = members.LastName,
                PhoneNumber = members.PhoneNumber,
                ProfileImageUrl = members.ProfileImageUrl,


            }).ToList()
        };

        await PopulateMembersAsync(viewModel);
        return View("Index", viewModel);
    }

    [HttpPost("Update/{id}")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Update(TrainingSessionViewModel viewModel)
    {
        if (!ModelState.IsValid)
        {
            await PopulateMembersAsync(viewModel);
            return View("Index", viewModel);
        }

        var input = new UpdateTrainingSessionInput
            (
                viewModel.Id,
                viewModel.Form.TrainerMemberId,
                viewModel.Form.SessionName,
                viewModel.Form.StartDate,
                viewModel.Form.EndDate,
                viewModel.Form.Capacity,
                viewModel.Form.Location!
            );

        var result = await updateTrainingSessionService.ExecuteAsync(input);
        if (!result.Success)
        {
            TempData["ErrorMessage"] = result.ErrorMessage;
            return View("index", viewModel);
        }

        TempData["SuccessMessage"] = "The training session was updated!";

        return RedirectToAction("Index", viewModel);
    }

    [HttpPost("Delete/{id}")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(Guid id)
    {
        if (id == Guid.Empty)
            return BadRequest("Invalid ID.");

        await deleteTrainingSessionService.ExecuteAsync(id);

        return RedirectToAction("Index");
    }

    /* 
     * ------------! Help by chatGPT to solve a problem !----------------
     * 
     *  I had a problem where if there was a validation error in the form,
     *  the member list was empty because when the page was rendered the other time,
     *  the members was not loaded from the memberQueryService because the metod was only applied i the Index-Action
     */ 

    private async Task PopulateMembersAsync(TrainingSessionViewModel viewModel)
    {
        var members = await memberQueryService.GetAllMembersAsync();
        viewModel.Members = members.Select(members => new MemberListViewModel
        {
            Id = members.Id,
            UserId = members.UserId,
            FirstName = members.FirstName,
            LastName = members.LastName,
            PhoneNumber = members.PhoneNumber,
            ProfileImageUrl = members.ProfileImageUrl,


        }).ToList();
    }
}