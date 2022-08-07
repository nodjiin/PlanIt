using Microsoft.EntityFrameworkCore;
using PlanIt.Application.Contracts.Persistence;
using PlanIt.Domain.Entities;

namespace PlanIt.Persistence.Repositories;
public class UserRepository : BaseRepository<User>, IUserRepository
{
    public UserRepository(PlanItDbContext dbContext) : base(dbContext)
    {
    }

    public async Task<User?> GetFullUserByIdAsync(Guid id, CancellationToken token = default)
    {
        if (DbContext.Users is null) return null;
        return await DbContext.Users.Include(u => u.Availabilities).SingleOrDefaultAsync(u => u.Id == id, token).ConfigureAwait(false);
    }
}
