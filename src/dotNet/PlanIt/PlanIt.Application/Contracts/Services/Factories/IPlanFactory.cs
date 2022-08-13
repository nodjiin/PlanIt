using PlanIt.Application.Dtos.Plan;
using PlanIt.Domain.Entities;

namespace PlanIt.Application.Contracts.Services.Factories;
public interface IPlanFactory : IEntityFactory<Plan, CreatePlanDto>
{
}
