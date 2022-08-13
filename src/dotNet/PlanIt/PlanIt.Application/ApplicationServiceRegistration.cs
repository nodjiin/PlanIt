using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PlanIt.Application.Contracts.Services.Factories;
using PlanIt.Application.Services.Factories;

namespace PlanIt.Application;
public static class ApplicationServiceRegistration
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IUserFactory, UserFactory>();
        services.AddScoped<IPlanFactory, PlanFactory>();
        return services;
    }
}
