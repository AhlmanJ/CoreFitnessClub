namespace Domain.Persistence.Repositories
{
    public interface IRepositoryBase<TDomainModel, TId>
    {
        Task AddAsync(TDomainModel model, CancellationToken ct = default);
        Task<IReadOnlyList<TDomainModel>> GetAllAsync(CancellationToken ct = default);
        Task<TDomainModel?> GetByIdAsync(TId id, CancellationToken ct = default);
        Task<bool> RemoveAsync(TDomainModel model, CancellationToken ct = default);
        Task<bool> UpdateAsync(TDomainModel model, CancellationToken ct = default);
    }
}