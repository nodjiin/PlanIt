using Microsoft.EntityFrameworkCore;
using PlanIt.Application.Contracts.Persistence;
using PlanIt.Domain.Entities;

namespace PlanIt.Persistence.Repositories;
public class PlanRepository : BaseRepository<Plan>, IPlanRepository
{
    public PlanRepository(PlanItDbContext dbContext) : base(dbContext)
    {
    }

    public async Task<Plan?> GetFullPlanByIdAsync(Guid id, CancellationToken token)
    {
        if (DbContext.Plans is null) return null;

        return await DbContext.Plans.Include(p => p.Users)
            .ThenInclude(u => u.Availabilities)
            .SingleOrDefaultAsync(p => p.Id == id, token)
            .ConfigureAwait(false);
    }
}
