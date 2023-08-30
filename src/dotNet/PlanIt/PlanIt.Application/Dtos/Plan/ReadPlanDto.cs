using PlanIt.Application.Dtos.User;

namespace PlanIt.Application.Dtos.Plan;
public class ReadPlanDto
{
    public Guid PlanId { get; set; }
    public List<ReadUserDto>? Users { get; set; }
    public DateTime FirstSchedulableDate { get; set; }
    public DateTime LastSchedulableDate { get; set; }
}
