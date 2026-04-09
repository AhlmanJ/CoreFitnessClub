using Application.Extensions;
using Infrastructure.Extensions;
using Infrastructure.Persistence;
using Microsoft.AspNetCore.Authentication;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();
builder.Services.AddRouting(options =>
{
    options.LowercaseUrls = true;
});
builder.Services.AddSession();

builder.Services.AddAuthentication()
    .AddGoogle(options =>
    {
        options.ClientId = builder.Configuration["Authentication:Google:ClientId"] ?? throw new InvalidOperationException("Google ClientId configuration is missing");
        options.ClientSecret = builder.Configuration["Authentication:Google:ClientSecret"] ?? throw new InvalidOperationException("Google ClientSecret configuration is missing");
    })
    .AddGitHub(options =>
    {
        options.ClientId = builder.Configuration["Authentication:GitHub:ClientId"] ?? throw new InvalidOperationException("GitHub ClientId configuration is missing");
        options.ClientSecret = builder.Configuration["Authentication:GitHub:ClientSecret"] ?? throw new InvalidOperationException("GitHub ClientSecret configuration is missing");
        options.Scope.Add("user:email"); // To get the email address from GitHub, you need to add this option. GitHub does not send the email address by default.
    });

builder.Services.AddInfrastructure(builder.Configuration, builder.Environment);
builder.Services.AddApplication(builder.Configuration, builder.Environment);

var app = builder.Build();

await PersistenceDatabaseInitializer.InitializeAsync(app.Services, app.Environment);

app.UseHsts();

app.UseStatusCodePagesWithReExecute("/Error/{0}");

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

// Sessions are used to, for example, get the email address from one view to another by using a string-key value.
app.UseSession();
app.UseAuthentication();
app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();
