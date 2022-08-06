using Microsoft.EntityFrameworkCore;
using PlanIt.Application.Contracts.Persistence;

namespace PlanIt.Persistence.Repositories;

public class BaseRepository<T> : IAsyncRepository<T>
    where T : class
{
    public BaseRepository(PlanItDbContext dbContext)
    {
        DbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
    }

    protected PlanItDbContext DbContext { get; }

    public virtual async Task<T?> GetByIdAsync(Guid id, CancellationToken token = default)
    {
        return await DbContext.Set<T>().FindAsync(new object?[] { id }, token).ConfigureAwait(false);
    }

    public async Task<IReadOnlyList<T>> ListAllAsync(CancellationToken token)
    {
        return await DbContext.Set<T>().ToListAsync(token).ConfigureAwait(false);
    }

    public virtual async Task<IReadOnlyList<T>> GetPagedResponseAsync(int page, int size, CancellationToken token = default)
    {
        if (page <= 0) throw new ArgumentException($"Argument {nameof(page)} can't a negative number");
        if (size <= 0) throw new ArgumentException($"Argument {nameof(size)} can't be 0 or a negative number");

        return await DbContext.Set<T>().Skip((page - 1) * size).Take(size).AsNoTracking().ToListAsync(token).ConfigureAwait(false);
    }

    public async Task<T> AddAsync(T entity, CancellationToken token = default)
    {
        if (entity is null) throw new ArgumentNullException(nameof(entity));

        await DbContext.Set<T>().AddAsync(entity, token).ConfigureAwait(false);
        await DbContext.SaveChangesAsync(token).ConfigureAwait(false);

        return entity;
    }

    public async Task UpdateAsync(T entity, CancellationToken token = default)
    {
        if (entity is null) throw new ArgumentNullException(nameof(entity));

        DbContext.Entry(entity).State = EntityState.Modified;
        await DbContext.SaveChangesAsync(token).ConfigureAwait(false);
    }

    public async Task DeleteAsync(T entity, CancellationToken token = default)
    {
        if (entity is null) throw new ArgumentNullException(nameof(entity));

        DbContext.Set<T>().Remove(entity);
        await DbContext.SaveChangesAsync(token).ConfigureAwait(false);
    }
}