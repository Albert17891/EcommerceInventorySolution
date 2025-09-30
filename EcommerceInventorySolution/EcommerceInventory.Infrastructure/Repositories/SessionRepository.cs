using EcommerceInventory.Application.RepositoryContracts;
using EcommerceInventory.Domain.Entities;
using EcommerceInventory.Infrastructure.DataContext;

namespace EcommerceInventory.Infrastructure.Repositories;
public class SessionRepository : ISessionRepository
{
    private readonly AppDbContext _context;

    public SessionRepository(AppDbContext context)
    {
        _context = context;
    }
    public async Task AddSessionAsync(Session session)
    {
        await _context.Sessions.AddAsync(session);
    }
}
