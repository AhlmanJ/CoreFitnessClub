
// help from chatGPT with debugging. See row 89.

using Application.Abstraction.BookingsInterface;
using Application.Abstraction.MembersQueryInterface;
using Application.Abstraction.TrainingSessionsInterface;
using Application.Bookings.Inputs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Presentation.WebApp.ViewModels;

namespace Presentation.WebApp.Controllers;

[Authorize]
[Route("TrainingSessions")]
public class TrainingSessionsController(ICreateBookingService createBookingService, IGetAllTrainingSessionsService getAllTrainingSessionsService, IMemberQueryService memberQueryService) : Controller
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
                TrainerMemberId = session.TrainerMemberId,
                TrainerFirstName = session.TrainerFirstName,
                TrainerLastName = session.TrainerLastName,
                SessionName = session.SessionName,
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
        return View();
    }

    [HttpPost("Create")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Guid id, Guid TrainerMemberId)
    {
        if (!ModelState.IsValid)
            return RedirectToAction("Index");

        var input = new CreateBookingInput
            (
                id, 
                TrainerMemberId
            );

        var result = await createBookingService.ExecuteAsync(input);

        /*
         * My page crashed if i already had a booking but i couldn't find the error, so i had to get AI to help with this.
         * The problem was that instead of redirecting to my Index Action, i was trying to return the View.
         */

        if (!result.Success)
        {
            TempData["ErrorMessage"] = result.ErrorMessage;
            return RedirectToAction("Index");
        }

        TempData["SuccessMessage"] = "Training Session Booked";
        return RedirectToAction("Index");
    }
}
