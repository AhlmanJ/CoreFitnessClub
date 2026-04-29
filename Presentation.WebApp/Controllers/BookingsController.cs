using Application.Abstraction.BookingsInterface;
using Application.Common.Roles;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace Presentation.WebApp.Controllers;

[Authorize]
[Route("Bookings")]
public class BookingsController(IDeleteBookingService deleteBookingService) : Controller
{
    [HttpGet("Booking")]
    public async Task<IActionResult> Booking()
    {
        
        return View();
    }

    [HttpPost("Delete/{id}")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(Guid id)
    {
        if (id == Guid.Empty)
            return BadRequest("Invalid ID.");

        await deleteBookingService.ExecuteAsync(id);

        return RedirectToAction("My","Account");
    }
}
