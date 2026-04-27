
/*
    On this page I got help from chatGPT on how I could render different partials depending on which link in the side menu I choose.
    It was difficult to find any information online about what I could do in this particular situation.
    I also asked about the difference between ViewBag and TempData.
 */

using Application.Abstraction.BookingsQueryInterface;
using Application.Abstraction.MembershipInterface;
using Application.Abstraction.MembersInterface;
using Application.Common.Roles;
using Application.Members.Inputs;
using Infrastructure.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Presentation.WebApp.Models.AccountModels;
using Presentation.WebApp.ViewModels;
using System.Data;

namespace Presentation.WebApp.Controllers;

[Authorize]
[Route("Account")]
public class AccountController (UserManager<ApplicationUser> userManager, IBookingsQueryService bookingsQueryService, IGetMemberProfileService getMemberProfileService, IUpdateMemberProfileService updateMemberProfileService, IGetMembershipByUserIdService getMembershipByUserIdService , IWebHostEnvironment _env) : Controller
{
    [HttpGet("my")]
    public async Task<IActionResult> My(string section = "about", CancellationToken ct = default)
    {
        var user = await userManager.GetUserAsync(User);
        if (user is null)
            return Challenge();

        var profile = await getMemberProfileService.ExecuteAsync(user.Id, ct);
        if (profile is null)
            return NotFound();

        var membershipResult = await getMembershipByUserIdService.ExecuteAsync(user.Id, ct);

        var membership = membershipResult?.Value;

        var viewModel = new MyAccountViewModel
        {
            Email = user.Email ?? string.Empty,
            AboutMeForm = new MyProfileForm
            {
                FirstName = profile.Value?.FirstName ?? string.Empty,
                LastName = profile.Value?.LastName ?? string.Empty,
                PhoneNumber = profile.Value?.PhoneNumber ?? string.Empty,
                ProfileImageUrl = profile.Value?.ProfileImageUrl ?? string.Empty
            },

            Membership = membership is null
            ? null
            : new MembershipViewModel
            {
                MemberId = membership.memberId,
                FirstName = membership.FirstName,
                LastName = membership.LastName,
                Name = membership.Name,
                Price = membership.Price,
                StartDate = membership.StartDate,
                EndDate = membership.EndDate
            }
        };

        if (section == "booking")
        {
            var member = await getMemberProfileService.ExecuteAsync(user.Id);
            if (member == null)
                return Challenge();

            var memberId = member.Value!.Id;

            var bookings = await bookingsQueryService.GetBookingsByMemberIdAsync(memberId);

            viewModel.Bookings = bookings.Select(b => new BookingsViewModel
            {
                Id = b.Id,
                SessionName = b.SessionName,
                TrainerFirstName = b.TrainerFirstName,
                TrainerLastName = b.TrainerLastName,
                StartDate = b.StartDate,
                EndDate = b.EndDate
            }).ToList();

        }

        ViewBag.Section = section;

        return View(viewModel);
    }

    [HttpPost("my")]
    public async Task<IActionResult> My(MyAccountViewModel viewModel, string section = "about", CancellationToken ct = default)
    {
        ViewBag.Section = section; // I was having problems with form validation. I learned that if i don't specify "ViewBag.Section" then the controller won't know which partial (section) to render after Post.

        if (!ModelState.IsValid)
            return View(viewModel);

        var user = await userManager.GetUserAsync(User);
        if (user is null)
            return Challenge();

        viewModel.Email = user.Email ?? string.Empty;

        string? profileImageUrl = viewModel.AboutMeForm.ProfileImageUrl; // Check if there is already a saved profile picture.

        if (viewModel.AboutMeForm.File != null && viewModel.AboutMeForm.File.Length > 0) // If a new profile picture has been uploaded.
        {
            var uploadFolder = Path.Combine(_env.WebRootPath, "profilePictures"); // Create a folder named "profilePictures" in wwwroot.
            Directory.CreateDirectory(uploadFolder);                              // Ensure the folder exists.

            var fileName = $"{Guid.NewGuid()}_{Path.GetFileName(viewModel.AboutMeForm.File.FileName)}"; // Combine the file name with a Guid number to make it unique within the folder and database.
            var filePath = Path.Combine(uploadFolder, fileName); // Create the "full" path where the image should be saved by combining the folder name and the filename.

            using (var stream = new FileStream(filePath, FileMode.Create)) // Text from chatGPT: FileStream is an object that represents a file and allows you to write to (or read from) it.
            {
                await viewModel.AboutMeForm.File.CopyToAsync(stream, ct);  // Copy the uploaded file's content to the server.
            }

            profileImageUrl = $"/profilePictures/{fileName}"; // Save the new file name to the property to save it to the database.
        }

        viewModel.AboutMeForm.ProfileImageUrl = profileImageUrl;

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
            TempData["ErrorMessage"] = result.ErrorMessage;
            return RedirectToAction("my",viewModel);
        }

        TempData["SuccessMessage"] = "The profile was updated!";
        return RedirectToAction("my",viewModel);
    }
}
