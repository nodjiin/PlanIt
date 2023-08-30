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

    public static ReadPlanDto ConvertToDto(this Plan plan)
    {
        ReadPlanDto dto = new ReadPlanDto();
        dto.Id = plan.Id;
        dto.FirstSchedulableDate = plan.FirstSchedulableDate;
        dto.LastSchedulableDate = plan.LastSchedulableDate;
        if (plan.Users != null)
        {
            dto.Users = plan.Users.Select(u => u.ConvertToDto()).ToList();
        }

        return dto;
    }
}
