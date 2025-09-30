using EcommerceInventory.Application.ServiceContracts;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace EcommerceInventory.Infrastructure.BackgroundServices;
public class QueuedHostedService : BackgroundService
{
    private readonly IBackgroundTaskQueue _taskQueue;
    private readonly ILogger<QueuedHostedService> _logger;
    private readonly IServiceScopeFactory _scopeFactory;

    public QueuedHostedService(IBackgroundTaskQueue taskQueue,
                               ILogger<QueuedHostedService> logger,
                               IServiceScopeFactory scopeFactory)
    {
        _taskQueue = taskQueue;
        _logger = logger;
        _scopeFactory = scopeFactory;
    }
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Background task queue service running.");

        await foreach (var workItem in _taskQueue.DequeueAsync(stoppingToken))
        {
            try
            {
                using var scope = _scopeFactory.CreateScope();
                await workItem(scope.ServiceProvider, stoppingToken);
            }
            catch (Exception ex)
            {

                _logger.LogError(ex, "Error occurred executing background task.");
            }
           
        }
    }
}
