using EcommerceInventory.Domain.Entities;

namespace EcommerceInventory.Application.RepositoryContracts;
public interface IOutboxRepository
{
    Task AddMessageAsync(OutboxMessage outboxMessage);
    Task<List<OutboxMessage>> GetUnprocessedMessagesAsync(int quantity);
}
