using PlanIt.Application.Contracts.Persistence;
using PlanIt.Domain.Entities;

namespace PlanIt.Persistence.Mocked.Repositories;
public class InMemoryUserRepository : IUserRepository
{
    public Task<User> AddAsync(User entity, CancellationToken token = default)
    {
        throw new NotImplementedException();
    }

    public Task DeleteAsync(User entity, CancellationToken token = default)
    {
        throw new NotImplementedException();
    }

    public Task<User?> GetByIdAsync(Guid id, CancellationToken token = default)
    {
        throw new NotImplementedException();
    }

    public Task<IReadOnlyList<User>> GetPagedResponseAsync(int page, int size, CancellationToken token = default)
    {
        throw new NotImplementedException();
    }

    public Task<IReadOnlyList<User>> ListAllAsync(CancellationToken token = default)
    {
        throw new NotImplementedException();
    }

    public Task UpdateAsync(User entity, CancellationToken token = default)
    {
        throw new NotImplementedException();
    }
}
