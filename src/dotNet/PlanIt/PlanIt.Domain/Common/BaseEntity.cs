namespace PlanIt.Domain.Common;

// TODO I should really ask myself if this little smartass think is worth it,
// or just create confusion in the long run. Entity Framework is smart enough to 
// understand which property is the right key for each table, but maybe deleting this class
// and add PlanId, UserId e AvailabilityId is just better...
public class BaseEntity
{
    public Guid Id { get; set; }
}