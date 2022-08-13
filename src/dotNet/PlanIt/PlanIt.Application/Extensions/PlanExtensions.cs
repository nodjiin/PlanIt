using PlanIt.Application.Dtos.Plan;
using PlanIt.Domain.Entities;

namespace PlanIt.Application.Extensions;
public static class PlanExtensions
{
    public static void Update(this Plan plan, UpdatePlanDto dto)
    {
        if (dto.UpdateFirstSchedulableDate) plan.FirstSchedulableDate = dto.FirstSchedulableDate;
        if (dto.UpdateLastSchedulableDate) plan.LastSchedulableDate = dto.LastSchedulableDate;
    }
}
