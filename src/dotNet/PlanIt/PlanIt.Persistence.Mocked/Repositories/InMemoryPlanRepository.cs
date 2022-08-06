using PlanIt.Application.Contracts.Persistence;
using PlanIt.Domain.Entities;

namespace PlanIt.Persistence.Mocked.Repositories;
public class InMemoryPlanRepository : IPlanRepository
{
    public Task<Plan> AddAsync(Plan entity, CancellationToken token = default)
    {
        throw new NotImplementedException();
    }

    public Task DeleteAsync(Plan entity, CancellationToken token = default)
    {
        throw new NotImplementedException();
    }

    public Task<Plan?> GetByIdAsync(Guid id, CancellationToken token = default)
    {
        throw new NotImplementedException();
    }

    public Task<IReadOnlyList<Plan>> GetPagedResponseAsync(int page, int size, CancellationToken token = default)
    {
        throw new NotImplementedException();
    }

    public Task<IReadOnlyList<Plan>> ListAllAsync(CancellationToken token = default)
    {
        throw new NotImplementedException();
    }

    public Task UpdateAsync(Plan entity, CancellationToken token = default)
    {
        throw new NotImplementedException();
    }
}
