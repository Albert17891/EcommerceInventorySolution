using EcommerceInventory.Application.RepositoryContracts;
using EcommerceInventory.Infrastructure.BackgroundServices;
using EcommerceInventory.Infrastructure.DataContext;
using EcommerceInventory.Infrastructure.Helper;
using EcommerceInventory.Infrastructure.RabbitMQ;
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
        services.AddScoped<IProductRepository, ProductRepository>();
        services.AddScoped<IOrderRepository, OrderRepository>();
        services.AddScoped<IOutboxRepository, OutboxRepository>();
        services.AddScoped<ISessionRepository, SessionRepository>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        services.AddSingleton<IRabbitMQPublisher, RabbitMQPublisher>();
        services.AddHostedService<OutboxProcessorService>();

        return services;
    }
}
