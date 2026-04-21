using Application.Abstraction;
using Infrastructure.Persistence.Contexts;

namespace Infrastructure.Persistence.UnitOfWork;

public sealed class UnitOfWork(DataContext context) : IUnitOfWork
{
    public Task CommitAsync(CancellationToken ct = default)
        => context.SaveChangesAsync(ct);
}
