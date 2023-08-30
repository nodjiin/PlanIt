using System.ComponentModel.DataAnnotations;

namespace PlanIt.Domain.Entities;
public class Availability
{
    [Key]
    public Guid AvailabilityId { get; set; }
    public Guid UserId { get; set; }
    public User? User { get; set; }
    public DateTime Date { get; set; }
}
