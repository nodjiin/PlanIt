using PlanIt.Domain.Common;

namespace PlanIt.Domain.Entities;
public class Plan : BaseEntity
{
    public List<User>? Users { get; set; }
    public DateTime FirstSchedulableDate { get; set; }
    public DateTime LastSchedulableDate { get; set; }
}
