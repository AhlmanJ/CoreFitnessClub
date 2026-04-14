using Application.Abstraction;
using Infrastructure.Persistence.Contexts;

namespace Infrastructure.Persistence.UnitOfWork;

public sealed class UnitOfWork : IUnitOfWork
{
    private readonly DataContext _context;

    public UnitOfWork(DataContext context)
    {
        _context = context;
    }

    public Task CommitAsync(CancellationToken ct = default)
        => _context.SaveChangesAsync(ct);
}
