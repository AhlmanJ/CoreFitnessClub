using Application.Common.Results;

namespace Application.Abstraction.BookingsInterface;

public interface IDeleteBookingService
{
    Task<Result<string?>> ExecuteAsync(Guid id, CancellationToken ct = default);
}
