using EcommerceInventory.Application.DTO;
using EcommerceInventory.Domain.Entities;

namespace EcommerceInventory.Application.RepositoryContracts;
public interface IUserRepository
{
    Task<User?> GetUserWithCurrentSessionAsync(Guid userId, Guid sessionId);
    Task<User?> GetUserWithAllActiveSessionsAsync(Guid userId);
    Task<User?> GetByUsernameAsync(string username);
    Task AddAsync(User user);
    void Update(User user);
}
