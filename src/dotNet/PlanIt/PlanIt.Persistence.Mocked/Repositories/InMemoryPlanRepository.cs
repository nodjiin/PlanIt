using PlanIt.Application.Contracts.Persistence;
using PlanIt.Domain.Entities;
using System.Collections.Concurrent;

namespace PlanIt.Persistence.Mocked.Repositories;
public class InMemoryPlanRepository : IPlanRepository
{
    private readonly ConcurrentDictionary<Guid, Plan> _plans = new();
    private readonly IUserRepository _userRepository;
    public InMemoryPlanRepository(IUserRepository userRepository)
    {
        Plan plan1 = new()
        {
            PlanId = Guid.NewGuid()
        };
        Plan plan2 = new()
        {
            PlanId = Guid.NewGuid()
        };

        _plans.TryAdd(plan1.PlanId, plan1);
        _plans.TryAdd(plan2.PlanId, plan2);
        _userRepository = userRepository;
    }

    public Task<Plan> AddAsync(Plan entity, CancellationToken token = default) => Task.Run(() =>
    {
        _plans.TryAdd(entity.PlanId, entity);
        return entity;
    });

    public Task DeleteAsync(Plan entity, CancellationToken token = default) => Task.Run(() =>
    {
        _plans.TryRemove(entity.PlanId, out var _);
    });

    public Task<Plan?> GetByIdAsync(Guid id, CancellationToken token = default)
    {
        _plans.TryGetValue(id, out var plan);
        return Task.FromResult(plan);
    }

    public async Task<Plan?> GetFullPlanByIdAsync(Guid id, CancellationToken token = default)
    {
        _plans.TryGetValue(id, out var plan);
        if (plan is null) return null;
        plan.Users = (await _userRepository.ListAllAsync().ConfigureAwait(false)).Where(u => u.PlanId == id).ToList();
        return plan;
    }

    public Task<IReadOnlyList<Plan>> GetPagedResponseAsync(int page, int size, CancellationToken token = default) => Task.Run(() =>
    {
        return (IReadOnlyList<Plan>)_plans.Values.Skip((page - 1) * size).Take(size).ToList();
    });

    public Task<Plan?> GetPlanWithUsersAndAvailabilitiesAsync(Guid id, CancellationToken token = default)
    {
        throw new NotImplementedException();
    }

    public Task<Plan?> GetPlanWithUsersAsync(Guid id, CancellationToken token = default)
    {
        throw new NotImplementedException();
    }

    public Task<IReadOnlyList<Plan>> ListAllAsync(CancellationToken token = default)
    {
        return Task.FromResult((IReadOnlyList<Plan>)_plans.Values.ToList());
    }

    public Task UpdateAsync(Plan entity, CancellationToken token = default) => Task.Run(() =>
    {
        _plans.TryGetValue(entity.PlanId, out var comparison);
        if (comparison is null) return;
        _plans.TryUpdate(entity.PlanId, entity, comparison);
    });
}
