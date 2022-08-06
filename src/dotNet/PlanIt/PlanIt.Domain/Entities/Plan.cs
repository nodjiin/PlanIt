using PlanIt.Domain.Common;

namespace PlanIt.Domain.Entities;
public class Plan : BaseEntity
{
    public List<User>? Users { get; set; }
    public List<DateTime>? SchedulableDates { get; set; }
}
