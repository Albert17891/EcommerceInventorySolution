using EcommerceInventory.Application.RepositoryContracts;
using EcommerceInventory.Infrastructure.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace EcommerceInventory.Infrastructure;
public static class ServiceCollectionExtensions
{
    // Extension method to add infrastructure services to the IServiceCollection
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services)
    {
        // Register infrastructure services here
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IUnitOfWOrk, UnitOfWork>();

        return services;
    }
}
