using PlanIt.Application.Dtos.Availability;
using PlanIt.Domain.Entities;

namespace PlanIt.Application.Extensions;
public static class AvailabilityExtensions
{
    public static ReadAvailabilityDto ConvertToDto(this Availability availability)
    {
        ReadAvailabilityDto dto = new ReadAvailabilityDto();
        dto.Date = availability.Date;
        return dto;
    }
}
