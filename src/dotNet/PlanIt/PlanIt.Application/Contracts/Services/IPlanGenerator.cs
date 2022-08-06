using PlanIt.Domain.Entities;

namespace PlanIt.Application.Contracts.Services;
public interface IPlanGenerator
{
    Plan CreatePlan(DateTime FirstSchedulableDate, DateTime LastSchedulableDate);
}
