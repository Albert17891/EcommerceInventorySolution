using EcommerceInventory.Application.ServiceContracts;
using EcommerceInventory.Application.Services;
using Microsoft.Extensions.DependencyInjection;

namespace EcommerceInventory.Application;
public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        // Register application services here
        services.AddScoped<IUserService, UserService>();

        return services;
    }
}
