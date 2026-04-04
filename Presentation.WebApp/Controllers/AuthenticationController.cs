
// I got support with the DeleteUser controller from chatGpt, i got help with explanation about session cookies and how to get user information from ApplicationUser.


using Application.Abstraction;
using Application.Abstraction.MembersInterface;
using Application.Members.Inputs;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Presentation.WebApp.Models.AuthenticationModels;

namespace Presentation.WebApp.Controllers;

[Route("authentication")]
public class AuthenticationController(IIdentityService identityService, IRegisterMemberAccountService registerMemberAccountService, ISignInMemberService signInMemberService) : Controller
{

    // A constant string that is needed for our HttpContext session to store a string-key value such as an Email address when you want to get it from one view to another.
    private const string RegisterEmailSessionKey = "RegistrationEmail";

    [HttpGet("sign-in")]
    public IActionResult SignIn()
    {
        return View(new SignInForm());
    }

    [HttpPost("sign-in")]
    [ValidateAntiForgeryToken] // Used to prevent anyone else from submitting a form in "our" name.
    public async Task<IActionResult> SignIn(SignInForm form, CancellationToken ct = default)
    {
        if (!ModelState.IsValid)
            return View(form);

        if (!form.AcceptTerms)
        {
            ModelState.AddModelError("AcceptTerms","You must accept the terms and conditions to continue");
            return View(form);
        }

        var input = new SignInInput(form.Email, form.Password, form.RememberMe);

        var result = await signInMemberService.ExecuteAsync(input, ct);
        if (!result.Success)
        {
            ViewData["ErrorMessage"] = result.ErrorMessage;
            return View(form);
        }

        return RedirectToAction("My", "Account");
    }

    [HttpPost("sign-out")]
    [ValidateAntiForgeryToken]
    public new async Task<IActionResult> SignOut()
    {
        await identityService.SignOutAsync();
        HttpContext.Session.Clear();

        return RedirectToAction("Index", "Home");
    }

    [HttpGet("sign-up")]
    public IActionResult SignUp()
    {
        return View(new RegisterEmailForm());
    }

    [HttpPost("sign-up")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> SignUp(RegisterEmailForm form, CancellationToken ct = default)
    {
        if (!ModelState.IsValid)
            return View(form);

        HttpContext.Session.SetString(RegisterEmailSessionKey, form.Email);

        return RedirectToAction(nameof(SetPassword));
    }

    [HttpGet("set-password")]
    public IActionResult SetPassword()
    {
        var email = HttpContext.Session.GetString(RegisterEmailSessionKey);
        if (string.IsNullOrWhiteSpace(email))
            return RedirectToAction(nameof(SignUp));

        return View(new RegisterPasswordForm());
    }

    [HttpPost("set-password")]
    public async Task<IActionResult> SetPassword(RegisterPasswordForm form, CancellationToken ct = default)
    {
        var email = HttpContext.Session.GetString(RegisterEmailSessionKey);
        if (string.IsNullOrWhiteSpace(email))
            return RedirectToAction(nameof(SignUp));

        if (!ModelState.IsValid)
            return View(form);

        if (!form.AcceptTerms)
        {
            ModelState.AddModelError("AcceptTerms", "You must accept the terms and conditions to continue");
            return View(form);
        }

        var registerMemberInput = new RegisterMemberAccountInput(email, form.Password);

        var registerResult = await registerMemberAccountService.ExecuteAsync(registerMemberInput, ct);
        if (!registerResult.Success)
        {
            ViewData["ErrorMessage"] = registerResult.ErrorMessage;
            return View(form);
        }

        var signInMemberInput = new SignInInput(email, form.Password, false);

        var signInResult = await signInMemberService.ExecuteAsync(signInMemberInput, ct);
        if (!signInResult.Success)
        {
            ViewData["ErrorMessage"] = "The account was created, but sign in failed";
            return View(form);
        }

        HttpContext.Session.Remove(RegisterEmailSessionKey);
        return RedirectToAction("My", "Account");
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteUser(CancellationToken ct = default)
    { 
        var email = User.Identity?.Name; // Get "UserName" from ApplicationUser. UserName is Email.

        if (string.IsNullOrWhiteSpace(email))
            return BadRequest("Could not find the user's email address, please try again.");

        var deleteUser = await identityService.DeleteUserAsync(email, ct); // Deletes user through the method in IdentityService.

        if (!deleteUser.Success)
        { 
            return BadRequest("Something went wrong! The user could not be deleted, please try again.");
        }

        HttpContext.Session.Clear(); // Removes session cookies.
        await HttpContext.SignOutAsync(); // Removes all login information including persistent data like "Remember Me".

        return RedirectToAction("Index", "Home");
    }

    /*
      
         
        ----> To register the profile, I don't know if I'll use this yet. <------
     
        public IActionResult RegisterProfile()
        {
            return View();
        }
    */
}
