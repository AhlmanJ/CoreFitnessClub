
/* 
 * I got support with the DeleteUser controller from chatGpt, i got help with explanation about session cookies and how to get user information from ApplicationUser.
 * Also how to configure HttpGet and Post to log in with an external authentication method.
 * I built this partly with the help of the lecture at school and support from chatGPT.
*/

using Application.Abstraction;
using Application.Abstraction.MembersInterface;
using Application.Members.Inputs;
using Infrastructure.Identity;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Presentation.WebApp.Models.AuthenticationModels;
using Presentation.WebApp.ViewModels;
using System.Security.Claims;

namespace Presentation.WebApp.Controllers;

[Route("authentication")]
public class AuthenticationController 
    (   IIdentityService identityService,
        IRegisterMemberAccountService registerMemberAccountService,
        ISignInMemberService signInMemberService,
        SignInManager<ApplicationUser> signInManager,
        UserManager<ApplicationUser> userManager,
        ILogger<AuthenticationController> logger
    ) : Controller

{

    // A constant string that is needed for our HttpContext session to store a string-key value such as an Email address when you want to get it from one view to another.
    private const string RegisterEmailSessionKey = "RegistrationEmail";

    [HttpGet("sign-in")]
    public async Task<IActionResult> SignIn(string? returnUrl = null)
    {
        var schemes = await signInManager.GetExternalAuthenticationSchemesAsync();

        var vm = new SignInViewModel
        {
            ReturnUrl = returnUrl,
            ExternalProviders = schemes.Select(x => x.Name).ToList(),
            Form = new SignInForm()
        };

        return View(vm);
    }

    // I got help from both the lecture at school and chatGPT on how to build this code-block!
    [HttpPost("sign-in")]
    [ValidateAntiForgeryToken] // Used to prevent anyone else from submitting a form in "our" name.
    public async Task<IActionResult> SignIn(SignInViewModel vm, CancellationToken ct = default)
    {
        var form = vm.Form;
        var returnUrl = vm.ReturnUrl;

        if (!ModelState.IsValid || !form.AcceptTerms)
        {
            if (!form.AcceptTerms)
                ModelState.AddModelError("AcceptTerms", "You must accept the terms and conditions to continue");

            
            var schemes = await signInManager.GetExternalAuthenticationSchemesAsync();
            var viewModel = new SignInViewModel
            {
                Form = form,
                ReturnUrl = returnUrl,
                ExternalProviders = schemes.Select(x => x.Name).ToList()
            };

            return View(viewModel);
        }
    

        var input = new SignInInput(form.Email, form.Password, form.RememberMe);
        var result = await signInMemberService.ExecuteAsync(input, ct);

        if (!result.Success)
        {
            ViewData["ErrorMessage"] = result.ErrorMessage;

            var schemes = await signInManager.GetExternalAuthenticationSchemesAsync();
            var viewModel = new SignInViewModel
            {
                Form = form,
                ReturnUrl = returnUrl,
                ExternalProviders = schemes.Select(x => x.Name).ToList()
            };

            return View(viewModel);
        }

        return RedirectToAction("My", "Account");
    }

    [HttpPost("external-login")]
    [ValidateAntiForgeryToken]
    public IActionResult ExternalLogin(string provider, string? returnUrl = null)
    {
        var redirectUrl = Url.Action(nameof(ExternalLoginCallback), "Authentication", new {returnUrl});
        var properties = signInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl);
        return Challenge(properties, provider);
    }

    [HttpGet("External-login-callback")]
    public async Task<IActionResult> ExternalLoginCallback(string? returnUrl = null, string? remoteError = null)
    {
        if (remoteError is not null)
        {
            logger.LogWarning("Remote error from provider: {Error}", remoteError);
            return ExternalLoginFailed(returnUrl);
        }

        var externalUser = await GetExternalUserInfo();
        if (externalUser is null)
            return ExternalLoginFailed(returnUrl);

        var (info, email) = externalUser.Value;

        var result = await signInManager.ExternalLoginSignInAsync
        (
            info.LoginProvider,
            info.ProviderKey,
            isPersistent: false,
            bypassTwoFactor: true
        );

        if (result.Succeeded)
            return RedirectToLocal(returnUrl);

        return await ExternalVerification(email, returnUrl);
    }

    private async Task<IActionResult> ExternalVerification(string email, string? returnUrl = null)
    {
        // TODO generate a verification code and send by email.

        return View("VerifyExternalLogin", new VerifyExternalLoginViewModel
        {
            ReturnUrl = returnUrl,
            Email = email
            
        });
    }

    // To be able to style the verification page in a simple way.

#if DEBUG

    [HttpGet("test-verify-external-login")]
    public IActionResult TestVerifyExternalLogin()
    {
        return View("VerifyExternalLogin", new VerifyExternalLoginViewModel
        {
            Email = "test@domain.com",
            ReturnUrl = "/"
        });
    }

#endif

    [HttpPost("verify-ExternalLogin-login")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> VerifyExternalLogin(VerifyExternalLoginViewModel vm)
    {
        if(!ModelState.IsValid)
            return View("VerifyExternalLogin",vm);

        // TODO! Validate the verification code.

        if (!string.Equals(vm.VerificationCode, "123456", StringComparison.Ordinal))
        {
            ModelState.AddModelError(nameof(vm.VerificationCode), "Wrong code.");
            return View("VerifyExternalLogin", vm);
        }

        var externalUser = await GetExternalUserInfo();
        if (externalUser is null)
            return ExternalLoginFailed(vm.ReturnUrl);

        var (info, email) = externalUser.Value;

        var existingUser = await userManager.FindByEmailAsync(email);
        if (existingUser is not null)
            return await LinkExistingUser(existingUser, info, vm.ReturnUrl);

        return await CreateExternalUser(email, info, vm.ReturnUrl);
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
    public async Task<IActionResult> SignUp(string? returnUrl = null)
    {
        var schemes = await signInManager.GetExternalAuthenticationSchemesAsync();

        var vm = new SignUpViewModel
        {
            ReturnUrl = returnUrl,
            ExternalProviders = schemes.Select(x => x.Name).ToList(),
            Form = new RegisterEmailForm()
        };

        return View(vm);
    }

    [HttpPost("sign-up")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> SignUp(SignUpViewModel vm, CancellationToken ct = default)
    {
        var form = vm.Form;
        var returnUrl = vm.ReturnUrl;

        // if error occurs you need to get the external provider again.
        if (!ModelState.IsValid)
        { 
            var schemes = await signInManager.GetExternalAuthenticationSchemesAsync();
            vm.ExternalProviders = schemes.Select(x => x.Name).ToList();

            return View(vm);
        }

        HttpContext.Session.SetString(RegisterEmailSessionKey, vm.Form.Email);
    
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

    [HttpPost("delete-user")]
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

        await signInManager.SignOutAsync(); // Removes all login information including persistent data like "Remember Me".
        HttpContext.Session.Clear(); // Removes session cookies.

        return RedirectToAction("Index", "Home");
    }



    private async Task<IActionResult> CreateExternalUser(string email, ExternalLoginInfo info, string? returnUrl = null)
    {
        var user = new ApplicationUser
        {
            UserName = email,
            Email = email,
            EmailConfirmed = true
        };

        var createResult = await userManager.CreateAsync(user);
        if (!createResult.Succeeded)
        {
            logger.LogError("Failed to create user {Email}: {Errors}",
                email,
                string.Join(",", createResult.Errors.Select(x => x.Description))
            );

            return ExternalLoginFailed(returnUrl);
        }

        var linkResult = await userManager.AddLoginAsync(user, info);
        if (!linkResult.Succeeded)
        {
            logger.LogError("Failed to link {Provider} to {Email} : {Errors}",
                    info.LoginProvider,
                    user.Email,
                    string.Join(",", linkResult.Errors.Select(x => x.Description))
                );

            return ExternalLoginFailed(returnUrl);
        }

        await signInManager.SignInAsync(user, isPersistent: false);
        return RedirectToLocal(returnUrl);
    }


    private async Task<IActionResult> LinkExistingUser(ApplicationUser user, ExternalLoginInfo info, string? returnUrl = null)
    {
        var result = await userManager.AddLoginAsync(user, info);
        if (!result.Succeeded)
        {
            logger.LogError("Failed to link {Provider} to {Email} : {Errors}",
                    info.LoginProvider,
                    user.Email,
                    string.Join(",", result.Errors.Select(x => x.Description))
                );

            return ExternalLoginFailed(returnUrl);
        }

        await signInManager.SignInAsync(user, isPersistent: false);
        return RedirectToLocal(returnUrl);

    }

    private async Task<(ExternalLoginInfo Info, string Email)?> GetExternalUserInfo()
    {
        var info = await signInManager.GetExternalLoginInfoAsync();
        if (info is null)
        {
            logger.LogWarning("External login info was null.");
            return null;
        }

        var email = info.Principal.FindFirstValue(ClaimTypes.Email);
        if (string.IsNullOrWhiteSpace(email))
        {
            logger.LogWarning("No email claim from {Provider}", info.LoginProvider);
            return null;
        }

        return (info, email);
    }



    private RedirectToActionResult ExternalLoginFailed(string? returnUrl = null)
    {
        TempData["Error"] = "Login failed, please try again";
        return RedirectToAction(nameof(SignIn), new { returnUrl });
    }

    private IActionResult RedirectToLocal(string? returnUrl = null)
    {
        if (Url.IsLocalUrl(returnUrl))
            return Redirect(returnUrl);

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
