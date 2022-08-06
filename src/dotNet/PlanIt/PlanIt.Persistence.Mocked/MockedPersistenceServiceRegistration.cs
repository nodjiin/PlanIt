using Microsoft.Extensions.DependencyInjection;
using PlanIt.Application.Contracts.Persistence;
using PlanIt.Persistence.Mocked.Repositories;

namespace PlanIt.Persistence.Mocked;

public static class MockedPersistenceServiceRegistration
{
    public static IServiceCollection AddMockedPersistenceServices(this IServiceCollection services)
    {
        services.AddSingleton<IUserRepository, InMemoryUserRepository>();
        services.AddSingleton<IPlanRepository, InMemoryPlanRepository>();

        return services;
    }
}