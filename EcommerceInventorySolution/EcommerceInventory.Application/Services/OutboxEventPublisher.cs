using EcommerceInventory.Application.RepositoryContracts;
using EcommerceInventory.Application.ServiceContracts;
using Microsoft.Extensions.Logging;

namespace EcommerceInventory.Application.Services;
public class OutboxEventPublisher : IEventPublisher
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<OutboxEventPublisher> _logger;

    public OutboxEventPublisher(IUnitOfWork unitOfWork, ILogger<OutboxEventPublisher> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }
    public async Task PublishAsync<T>(T @event) where T : class
    {
        var eventType = typeof(T).Name;
        var payload = System.Text.Json.JsonSerializer.Serialize(@event);

        var outboxMessage = new Domain.Entities.OutboxMessage(eventType, payload);

        await _unitOfWork.OutboxMessages.AddMessageAsync(outboxMessage);

        await _unitOfWork.CompleteAsync();

        _logger.LogInformation("Event {EventType} saved to outbox", eventType);
    }
}
