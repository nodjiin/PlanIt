using Microsoft.Extensions.DependencyInjection;
using PlanIt.Application.Contracts.Persistence;
using PlanIt.Persistence.Mocked.Repositories;

namespace PlanIt.Persistence.Mocked;

public static class PersistenceServiceRegistration
{
    public static IServiceCollection AddMockedPersistenceServices(this IServiceCollection services)
    {
        services.AddScoped<IUserRepository, InMemoryUserRepository>();
        services.AddScoped<IPlanRepository, InMemoryPlanRepository>();

        return services;
    }
}