using PlanIt.Domain.Entities;

namespace PlanIt.Application.Contracts.Persistence;
public interface IPlanRepository : IAsyncRepository<Plan>
{
}
