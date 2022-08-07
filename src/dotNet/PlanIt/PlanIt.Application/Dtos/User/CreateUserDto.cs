using PlanIt.Application.Dtos.Availability;

namespace PlanIt.Application.Dtos.User;
public class CreateUserDto
{
    public Guid PlanId { get; set; }
    public List<AvailabilityDto>? Availabilities { get; set; }
}
