namespace PlanIt.Application.Dtos.Plan;
public class UpdatePlanDto
{
    public Guid Id { get; set; }
    public List<Guid>? UsersToAdd { get; set; }
    public List<Guid>? UsersToRemove { get; set; }
}
