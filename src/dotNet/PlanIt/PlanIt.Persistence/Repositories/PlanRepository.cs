using PlanIt.Application.Contracts.Persistence;
using PlanIt.Domain.Entities;

namespace PlanIt.Persistence.Repositories;
public class PlanRepository : BaseRepository<Plan>, IPlanRepository
{
    public PlanRepository(PlanItDbContext dbContext) : base(dbContext)
    {
    }
}
