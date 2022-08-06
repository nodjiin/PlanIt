using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PlanIt.Application.Contracts.Services;
using PlanIt.Application.Services.Plan;

namespace PlanIt.Application;
public static class ApplicationServiceRegistration
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IPlanGenerator, PlanGenerator>();
        return services;
    }
}
