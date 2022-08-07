using PlanIt.Domain.Entities;

namespace PlanIt.Application.Contracts.Persistence;
public interface IPlanRepository : IAsyncRepository<Plan>
{
    Task<Plan?> GetFullPlanByIdAsync(Guid id, CancellationToken token = default);
}
