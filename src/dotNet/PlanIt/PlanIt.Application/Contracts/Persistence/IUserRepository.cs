using PlanIt.Domain.Entities;

namespace PlanIt.Application.Contracts.Persistence;
public interface IUserRepository : IAsyncRepository<User>
{
    Task<User?> GetFullUserByIdAsync(Guid id, CancellationToken token = default);
}
