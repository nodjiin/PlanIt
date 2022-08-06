using PlanIt.Application.Contracts.Services;

namespace PlanIt.Application.Services.Plan;
public class PlanGenerator : IPlanGenerator
{
    public Domain.Entities.Plan CreatePlan(DateTime FirstSchedulableDate, DateTime LastSchedulableDate)
    {
        List<DateTime> schedulableDateList = Enumerable.Range(0, 1 + LastSchedulableDate.Subtract(FirstSchedulableDate).Days)
          .Select(offset => FirstSchedulableDate.AddDays(offset)).ToList();

        return new()
        {
            Id = Guid.NewGuid(),
            SchedulableDates = schedulableDateList
        };
    }
}
