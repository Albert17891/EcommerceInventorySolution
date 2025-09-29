using EcommerceInventory.Domain.Entities;

namespace EcommerceInventory.Application.RepositoryContracts;
public interface IUserRepository
{
    Task<User?> GetByIdAsync(Guid id);
    Task<User?> GetByUsernameAsync(string username);
    Task AddAsync(User user);
    void Update(User user);
}
