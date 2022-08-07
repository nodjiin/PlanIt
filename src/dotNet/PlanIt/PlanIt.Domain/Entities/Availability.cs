using PlanIt.Domain.Common;

namespace PlanIt.Domain.Entities;
public class Availability : BaseEntity
{
    public Guid UserId { get; set; }
    public DateTime Date { get; set; }
}
