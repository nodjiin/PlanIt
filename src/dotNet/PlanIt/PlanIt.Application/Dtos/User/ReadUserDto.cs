using PlanIt.Application.Dtos.Availability;

namespace PlanIt.Application.Dtos.User;
public class ReadUserDto
{
    public Guid UserId { get; set; }
    public string? Name { get; set; }
    public List<ReadAvailabilityDto>? Availabilities { get; set; }
}
