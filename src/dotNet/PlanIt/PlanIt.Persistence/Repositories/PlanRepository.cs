using PlanIt.Application.Contracts.Persistence;
using PlanIt.Domain.Entities;

namespace PlanIt.Persistence.Repositories;
internal class PlanRepository : BaseRepository<Plan>, IPlanrepository
{
    public PlanRepository(PlanItDbContext dbContext) : base(dbContext)
    {
    }
}
