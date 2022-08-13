using PlanIt.Application.Contracts.Services.Factories;
using PlanIt.Application.Dtos.Plan;
using PlanIt.Domain.Entities;

namespace PlanIt.Application.Services.Factories;
public class PlanFactory : IPlanFactory
{
    public Plan Create(CreatePlanDto dto)
    {
        return new Plan { FirstSchedulableDate = dto.FirstSchedulableDate, LastSchedulableDate = dto.LastSchedulableDate };
    }
}
