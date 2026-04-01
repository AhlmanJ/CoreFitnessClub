using Application.Extensions;
using Infrastructure.Extensions;
using Infrastructure.Persistence;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();
builder.Services.AddRouting(options =>
{
    options.LowercaseUrls = true;
});
builder.Services.AddSession();

builder.Services.AddInfrastructure(builder.Configuration, builder.Environment);
builder.Services.AddApplication(builder.Configuration, builder.Environment);


var app = builder.Build();

await PersistenceDatabaseInitializer.InitializeAsync(app.Services, app.Environment);

app.UseHsts();
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
