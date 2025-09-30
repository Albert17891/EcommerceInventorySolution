using EcommerceInventory.Application.Mappings;
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
        services.AddScoped<IProductService, ProductService>();
        services.AddScoped<IOrderService, OrderService>();
        services.AddScoped<IPaymentService, PaymentService>();
        services.AddScoped<IEventPublisher, OutboxEventPublisher>();

        services.AddAutoMapper(cfg =>
        {
            cfg.AddProfile<ProductMappingProfile>();
        });

        return services;
    }
}
