namespace PlanIt.Application.Dtos.Plan;
public class UpdatePlanDto
{
    public Guid Id { get; set; }
    public bool UpdateFirstSchedulableDate { get; set; }
    public DateTime FirstSchedulableDate { get; set; }
    public bool UpdateLastSchedulableDate { get; set; }
    public DateTime LastSchedulableDate { get; set; }
}
