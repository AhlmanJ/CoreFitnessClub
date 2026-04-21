using Domain.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories;

public abstract class RepositoryBase<TDomainModel, Guid, TEntity, TDbContext>(TDbContext context) : IRepositoryBase<TDomainModel, Guid> where TEntity : class where TDbContext : DbContext
{
    protected readonly TDbContext _context = context;
    protected DbSet<TEntity> Set => _context.Set<TEntity>();
    protected abstract Guid GetId(TDomainModel model);
    protected abstract TEntity ToEntity(TDomainModel model);
    protected abstract TDomainModel ToDomainModel(TEntity entity);

    protected abstract void UpdateEntity(TEntity entity, TDomainModel model);


    public virtual async Task AddAsync(TDomainModel model, CancellationToken ct = default)
    {
        try
        {
            if (model is null)
                throw new ArgumentNullException(nameof(model));

            var entity = ToEntity(model);
            await Set.AddAsync(entity, ct);
            
        }
        catch
        {
            throw;
        }
    }

    public virtual async Task<bool> UpdateAsync(TDomainModel model, CancellationToken ct = default)
    {
        try
        {
            if (model is null)
                throw new ArgumentNullException(nameof(model));

            var id = GetId(model);

            var entity = await Set.FindAsync(id, ct);
            if (entity is null)
                return false;

            UpdateEntity(entity,model);
            
            return true;
        }
        catch
        {
            throw;
        }
    }

    public virtual async Task<bool> RemoveAsync(TDomainModel model, CancellationToken ct = default)
    {
        try
        {
            if (model is null)
                throw new ArgumentNullException(nameof(model));

            var id = GetId(model);

            var entity = await Set.FindAsync(id, ct);
            if (entity is null)
                return false;

            Set.Remove(entity);
            
            return true;
        }
        catch
        {
            throw;
        }
    }

    public virtual async Task<TDomainModel?> GetByIdAsync(Guid id, CancellationToken ct = default)
    {
        try
        {
            var entity = await Set.FindAsync(id, ct);
            return entity is null ? default : ToDomainModel(entity);
        }
        catch
        {
            throw;
        }
    }

    public virtual async Task<IReadOnlyList<TDomainModel>> GetAllAsync(CancellationToken ct = default)
    {
        try
        {
            var entities = await Set.AsNoTracking().ToListAsync(ct);
            return [.. entities.Select(ToDomainModel)];
        }
        catch
        {
            throw;
        }
    }
}
