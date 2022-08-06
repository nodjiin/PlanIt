namespace PlanIt.Application.Contracts.Persistence
{
    public interface IAsyncRepository<T> where T : class
    {
        Task<T?> GetByIdAsync(Guid id, CancellationToken token = default);
        Task<IReadOnlyList<T>> ListAllAsync(CancellationToken token = default);
        Task<T> AddAsync(T entity, CancellationToken token = default);
        Task UpdateAsync(T entity, CancellationToken token = default);
        Task DeleteAsync(T entity, CancellationToken token = default);
        Task<IReadOnlyList<T>> GetPagedResponseAsync(int page, int size, CancellationToken token = default);
    }
}
