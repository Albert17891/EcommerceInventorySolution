using EcommerceInventory.Application.RepositoryContracts;
using EcommerceInventory.Domain.Entities;
using EcommerceInventory.Infrastructure.DataContext;
using Microsoft.EntityFrameworkCore;

namespace EcommerceInventory.Infrastructure.Repositories;
public class UserRepository : IUserRepository
{
    private readonly AppDbContext _db;
    public UserRepository(AppDbContext db) => _db = db;

    public async Task<User?> GetByIdAsync(Guid id) => await _db.Users.FindAsync(id);
    public async Task<User?> GetByUsernameAsync(string username) =>
        await _db.Users.Include(u => u.Sessions)
                       .FirstOrDefaultAsync(u => u.Username == username);
    public async Task AddAsync(User user) => await _db.Users.AddAsync(user);
    public void Update(User user) => _db.Users.Update(user);
}
