using System.ComponentModel.DataAnnotations;

namespace PlanIt.Domain.Entities;
public class User
{
    [Key]
    public Guid UserId { get; set; }
    public string? Name { get; set; }
    public Guid PlanId { get; set; }
    public Plan? Plan { get; set; }
    public List<Availability>? Availabilities { get; set; }
}
