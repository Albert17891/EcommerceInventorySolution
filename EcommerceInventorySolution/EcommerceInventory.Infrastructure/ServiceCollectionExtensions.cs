using EcommerceInventory.Application.RepositoryContracts;
using EcommerceInventory.Infrastructure.DataContext;
using EcommerceInventory.Infrastructure.Helper;
using EcommerceInventory.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace EcommerceInventory.Infrastructure;
public static class ServiceCollectionExtensions
{
    // Extension method to add infrastructure services to the IServiceCollection
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = ConnectionStringHelper.GetPostgreConnectionString(configuration);

        services.AddDbContext<AppDbContext>(options =>
                                           options.UseNpgsql(connectionString));

        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IUnitOfWOrk, UnitOfWork>();

        return services;
    }
}
