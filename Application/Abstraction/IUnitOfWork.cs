namespace Application.Abstraction;

public interface IUnitOfWork
{
    Task CommitAsync(CancellationToken ct = default);
}
