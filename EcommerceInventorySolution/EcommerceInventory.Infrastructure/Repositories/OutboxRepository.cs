using EcommerceInventory.Application.RepositoryContracts;
using EcommerceInventory.Domain.Entities;
using EcommerceInventory.Infrastructure.DataContext;
using Microsoft.EntityFrameworkCore;

namespace EcommerceInventory.Infrastructure.Repositories;
public class OutboxRepository : IOutboxRepository
{
    private readonly AppDbContext _dbContext;

    public OutboxRepository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    public async Task AddMessageAsync(OutboxMessage outboxMessage)
    {
        await _dbContext.AddAsync(outboxMessage);
    }

    public async Task<List<OutboxMessage>> GetUnprocessedMessagesAsync(int quantity)
    {
        return await _dbContext.OutboxMessages
            .Where(m => !m.IsProcessed)
            .OrderBy(m => m.CreatedAt)
            .Take(quantity)
            .ToListAsync();
    }
}
