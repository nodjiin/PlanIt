using Microsoft.EntityFrameworkCore;
using PlanIt.Domain.Entities;

namespace PlanIt.Persistence;

#nullable disable 
public class PlanItDbContext : DbContext
{
    public PlanItDbContext(DbContextOptions<PlanItDbContext> options)
        : base(options)
    {
    }

    public DbSet<User> Users { get; set; }
    public DbSet<Plan> Plans { get; set; }
    public DbSet<Availability> Availabilities { get; set; }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder?.ApplyConfigurationsFromAssembly(typeof(PlanItDbContext).Assembly);
    }
}
#nullable enable