using System.ComponentModel.DataAnnotations;

namespace PlanIt.Domain.Entities;
public class Plan
{
    [Key]
    public Guid PlanId { get; set; }
    public List<User>? Users { get; set; }
    public DateTime FirstSchedulableDate { get; set; }
    public DateTime LastSchedulableDate { get; set; }
}
