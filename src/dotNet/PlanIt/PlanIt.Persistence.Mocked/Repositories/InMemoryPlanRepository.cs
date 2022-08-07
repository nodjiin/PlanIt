using PlanIt.Application.Contracts.Persistence;
using PlanIt.Domain.Entities;

namespace PlanIt.Persistence.Mocked.Repositories;
public class InMemoryPlanRepository : IPlanRepository
{
    private readonly Dictionary<Guid, Plan> _plans = new();

    public InMemoryPlanRepository()
    {
        Plan plan1 = new()
        {
            Id = Guid.NewGuid()
        };
        Plan plan2 = new()
        {
            Id = Guid.NewGuid()
        };

        _plans.Add(plan1.Id, plan1);
        _plans.Add(plan2.Id, plan2);
    }

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
        _plans.TryGetValue(id, out var plan);
        return Task.FromResult(plan);
    }

    public Task<Plan?> GetFullPlanByIdAsync(Guid id, CancellationToken token = default)
    {
        throw new NotImplementedException();
    }

    public Task<IReadOnlyList<Plan>> GetPagedResponseAsync(int page, int size, CancellationToken token = default)
    {
        throw new NotImplementedException();
    }

    public Task<IReadOnlyList<Plan>> ListAllAsync(CancellationToken token = default)
    {
        return Task.FromResult((IReadOnlyList<Plan>)_plans.Values.ToList());
    }

    public Task UpdateAsync(Plan entity, CancellationToken token = default)
    {
        throw new NotImplementedException();
    }
}
