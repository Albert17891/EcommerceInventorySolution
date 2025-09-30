using EcommerceInventory.Application.RepositoryContracts;
using EcommerceInventory.Application.ServiceContracts;
using EcommerceInventory.Domain.Entities;

namespace EcommerceInventory.Application.Services;
public class UserService : IUserService
{
    private readonly IUnitOfWork _unitOfWork;

    public UserService(IUnitOfWork unitOfWOrk)
    {
        _unitOfWork = unitOfWOrk;
    }

    public async Task<Session> LoginAsync(string username, string password, string deviceId)
    {
        var user = await _unitOfWork.Users.GetByUsernameAsync(username)
            ?? throw new InvalidOperationException("Invalid username or password.");

        if (!user.VerifyPassword(password))
            throw new InvalidOperationException("Invalid username or password.");

        var session = new Session(user.Id, deviceId, DateTime.UtcNow.AddHours(1));

        await _unitOfWork.Sessions.AddSessionAsync(session);

        await _unitOfWork.CompleteAsync();

        return session;
    }

    public async Task LogoutAllAsync(Guid userId)
    {
        var user = await _unitOfWork.Users.GetByIdAsync(userId)
            ?? throw new InvalidOperationException("User not found.");

        user.RemoveAllSessions();

        _unitOfWork.Users.Update(user);

        await _unitOfWork.CompleteAsync();
    }

    public async Task LogoutAsync(Guid userId, Guid sessionId)
    {
        var user = await _unitOfWork.Users.GetByIdAsync(userId)
            ?? throw new InvalidOperationException("User not found.");

        var session = user.Sessions.FirstOrDefault(s => s.Id == sessionId);
        if (session != null)
        {
            user.RemoveSession(session);
            _unitOfWork.Users.Update(user);
            await _unitOfWork.CompleteAsync();
        }
    }

    public async Task<User> RegisterUserAsync(string username, string password)
    {
        if (await _unitOfWork.Users.GetByUsernameAsync(username) is not null)
        {
            throw new InvalidOperationException("Username already exists.");
        }

        var user = new User(username, password);

        await _unitOfWork.Users.AddAsync(user);

        await _unitOfWork.CompleteAsync();

        return user;
    }
}
