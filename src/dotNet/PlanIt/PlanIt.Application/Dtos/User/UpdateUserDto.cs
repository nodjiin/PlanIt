using PlanIt.Application.Dtos.Availability;

namespace PlanIt.Application.Dtos.User;
public class UpdateUserDto
{
    public Guid Id { get; set; }
    public List<AvailabilityDto>? Availabilities { get; set; }
}
