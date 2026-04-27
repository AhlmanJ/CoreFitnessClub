using Application.Bookings.Inputs;
using Application.Bookings.Outputs;
using Application.Common.Results;

namespace Application.Abstraction.BookingsInterface;

public interface ICreateBookingService
{
    Task<Result<BookingsOutput>> ExecuteAsync(CreateBookingInput input, CancellationToken ct = default); 
}
