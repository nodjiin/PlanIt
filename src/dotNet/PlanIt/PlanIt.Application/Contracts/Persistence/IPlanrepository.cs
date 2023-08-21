using PlanIt.Domain.Entities;

namespace PlanIt.Application.Contracts.Persistence;
public interface IPlanRepository : IAsyncRepository<Plan>
{
    Task<Plan?> GetPlanWithUsersAsync(Guid id, CancellationToken token = default);

    Task<Plan?> GetPlanWithUsersAndAvailabilitiesAsync(Guid id, CancellationToken token = default);
}
