using PlanIt.Domain.Common;

namespace PlanIt.Domain.Entities;
public class User : BaseEntity
{
    public Guid PlanId { get; set; }
    public List<Availability> Availabilities { get; set; }
}
