using Microsoft.EntityFrameworkCore;
using PlanIt.Application.Contracts.Persistence;
using PlanIt.Domain.Entities;

namespace PlanIt.Persistence.Repositories;
public class PlanRepository : BaseRepository<Plan>, IPlanRepository
{
    public PlanRepository(PlanItDbContext dbContext) : base(dbContext)
    {
    }

    public async Task<Plan?> GetPlanWithUsersAsync(Guid id, CancellationToken token = default)
    {
        if (DbContext.Plans is null) return null;

        return await DbContext.Plans.Where(p => p.Id == id).Include(p => p.Users).FirstOrDefaultAsync(token).ConfigureAwait(false);
    }

    public async Task<Plan?> GetPlanWithUsersAndAvailabilitiesAsync(Guid id, CancellationToken token = default)
    {
        if (DbContext.Plans is null) return null;

#nullable disable // EF handles the nullability of the internal collections
        return await DbContext.Plans.Where(p => p.Id == id).Include(p => p.Users).ThenInclude(u => u.Availabilities).FirstOrDefaultAsync(token).ConfigureAwait(false);
#nullable enable
    }
}
