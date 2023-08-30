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
        modelBuilder?.Entity<User>().HasIndex(u => new { u.PlanId, u.Name }).IsUnique();
        modelBuilder?.Entity<User>().HasOne(u => u.Plan).WithMany(p => p.Users).HasForeignKey(u => u.PlanId).OnDelete(DeleteBehavior.Cascade);
        modelBuilder?.Entity<Availability>().HasOne(a => a.User).WithMany(u => u.Availabilities).HasForeignKey(a => a.UserId).OnDelete(DeleteBehavior.Cascade);
    }
}
#nullable enable