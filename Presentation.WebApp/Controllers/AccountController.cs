using Application.Abstraction.MembersInterface;
using Application.Members.Inputs;
using Infrastructure.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Presentation.WebApp.Models.AccountModels;

namespace Presentation.WebApp.Controllers;

[Authorize]
[Route("Account")]
public class AccountController ( UserManager<ApplicationUser> userManager, IGetMemberProfileService getMemberProfileService, IUpdateMemberProfileService updateMemberProfileService ) : Controller
{
    [HttpGet("my")]
    public async Task<IActionResult> My(CancellationToken ct = default)
    {
        var user = await userManager.GetUserAsync(User);
        if (user is null)
            return Challenge();

        var profile = await getMemberProfileService.ExecuteAsync(user.Id, ct);
        if (profile is null)
            return NotFound();

        var model = new MyAccountViewModel
        {
            AboutMeForm = new MyProfileForm
            {
                FirstName = profile.Value?.FirstName ?? string.Empty,
                LastName = profile.Value?.LastName ?? string.Empty,
                PhoneNumber = profile.Value?.PhoneNumber ?? string.Empty,
                ProfileImageUrl = profile.Value?.ProfileImageUrl ?? string.Empty
            }
        };

        return View();
    }

    [HttpPost("my")]
    public async Task<IActionResult> My(MyAccountViewModel viewModel, CancellationToken ct = default)
    {

        if (!ModelState.IsValid)
            return View(viewModel);

        var user = await userManager.GetUserAsync(User);
        if (user is null)
            return Challenge();

        var input = new UpdateMemberProfileInput
            (
                user.Id,
                viewModel.AboutMeForm.FirstName,
                viewModel.AboutMeForm.LastName,
                viewModel.AboutMeForm.PhoneNumber,
                viewModel.AboutMeForm.ProfileImageUrl
            );

        var result = await updateMemberProfileService.ExecuteAsync(input, ct);
        if (!result.Success)
        {
            ViewData["Message"] = result.ErrorMessage;
            ViewData["MessageType"] = "success";
            return View(viewModel);
        }

        return View();
    }
}
