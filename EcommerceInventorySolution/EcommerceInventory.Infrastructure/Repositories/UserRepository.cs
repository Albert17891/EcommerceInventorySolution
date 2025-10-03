using EcommerceInventory.Application.RepositoryContracts;
using EcommerceInventory.Domain.Entities;
using EcommerceInventory.Infrastructure.DataContext;
using Microsoft.EntityFrameworkCore;

namespace EcommerceInventory.Infrastructure.Repositories;
public class UserRepository : IUserRepository
{
    private readonly AppDbContext _db;
    public UserRepository(AppDbContext db) => _db = db;


    public async Task<User?> GetByUsernameAsync(string username) =>
        await _db.Users
                       .FirstOrDefaultAsync(u => u.Username == username);

    public async Task AddAsync(User user) => await _db.Users.AddAsync(user);

    public void Update(User user) => _db.Users.Update(user);

    public async Task<User?> GetUserWithCurrentSessionAsync(Guid userId, Guid sessionId)
    {
        return await _db.Users
             .Include(u => u.Sessions.Where(s => s.Id == sessionId && s.ExpiresAt > DateTime.UtcNow))
             .FirstOrDefaultAsync(u => u.Id == userId);
    }

    public async Task<User?> GetUserWithAllActiveSessionsAsync(Guid userId)
    {
        return await _db.Users
             .Include(u => u.Sessions.Where(s => s.ExpiresAt > DateTime.UtcNow))
             .FirstOrDefaultAsync(u => u.Id == userId);
    }
}
