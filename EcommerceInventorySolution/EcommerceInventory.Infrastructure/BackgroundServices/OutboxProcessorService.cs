using EcommerceInventory.Application.RepositoryContracts;
using EcommerceInventory.Domain.Entities;
using EcommerceInventory.Infrastructure.RabbitMQ;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace EcommerceInventory.Infrastructure.BackgroundServices;

public class OutboxProcessorService : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<OutboxProcessorService> _logger;

    public OutboxProcessorService(IServiceProvider serviceProvider, ILogger<OutboxProcessorService> logger)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
    }
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                await ProcessOutboxMessagesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing outbox messages");
            }

            await Task.Delay(TimeSpan.FromSeconds(10), stoppingToken);
        }
    }

    private async Task ProcessOutboxMessagesAsync()
    {
        using var scope = _serviceProvider.CreateScope();
        var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();

        var messages = await unitOfWork.OutboxMessages.GetUnprocessedMessagesAsync(10);

        foreach (var message in messages)
        {
            try
            {
                await SendToExternalServiceAsync(message);

                message.MarkAsProcessed();

                await unitOfWork.CompleteAsync();

                _logger.LogInformation("Processed outbox message {MessageId}", message.Id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing outbox message {MessageId}", message.Id);

                message.IncrementRetryCount();
                await unitOfWork.CompleteAsync();
            }
        }
    }

    private async Task SendToExternalServiceAsync(OutboxMessage message)
    {
        using var scope = _serviceProvider.CreateScope();
        var rabbitMQPublisher = scope.ServiceProvider.GetRequiredService<IRabbitMQPublisher>();

        var routingKey = message.EventType switch
        {
            "OrderCompletedEvent" => "order.completed",
            _ => "unknown.event"
        };

        var eventData = JsonSerializer.Deserialize<object>(message.Payload);

        await rabbitMQPublisher.Publish(routingKey, eventData);

        _logger.LogInformation("Sent event {EventType} to RabbitMQ", message.EventType);
    }
}
