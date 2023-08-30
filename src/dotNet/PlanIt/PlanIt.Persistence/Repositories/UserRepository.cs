using Microsoft.EntityFrameworkCore;
using PlanIt.Application.Contracts.Persistence;
using PlanIt.Domain.Entities;

namespace PlanIt.Persistence.Repositories;

#nullable disable
public class UserRepository : BaseRepository<User>, IUserRepository
{
    public UserRepository(PlanItDbContext dbContext) : base(dbContext)
    {
    }

    public async Task<User> GetFullUserByIdAsync(Guid id, CancellationToken token = default)
    {
        if (DbContext.Users is null) return null;
        return await DbContext.Users.Include(u => u.Availabilities.OrderBy(av => av.Date)).SingleOrDefaultAsync(u => u.UserId == id, token).ConfigureAwait(false);
    }
}
#nullable enable
