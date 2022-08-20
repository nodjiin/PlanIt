using PlanIt.Domain.Common;

namespace PlanIt.Domain.Entities;
public class User : BaseEntity
{
    public string? Name { get; set; }
    public Guid PlanId { get; set; }
    public List<Availability>? Availabilities { get; set; }
}
