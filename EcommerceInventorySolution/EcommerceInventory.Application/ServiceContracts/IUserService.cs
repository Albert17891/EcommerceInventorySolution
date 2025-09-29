using EcommerceInventory.Domain.Entities;

namespace EcommerceInventory.Application.ServiceContracts;
public interface IUserService
{

    Task<User> RegisterUserAsync(string username, string password);
    Task<Session> LoginAsync(string username, string password, string deviceId);
    Task LogoutAsync(Guid userId, Guid sessionId);
    Task LogoutAllAsync(Guid userId);
}
