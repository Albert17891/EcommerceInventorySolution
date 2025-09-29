namespace EcommerceInventory.Domain.Entities;

public class Session
{
    public Guid Id { get; private set; } = Guid.NewGuid();
    public Guid UserId { get; private set; }
    public string DeviceId { get; private set; }
    public string Token { get; private set; } = null!;
    public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;
    public DateTime ExpiresAt { get; private set; }

    public User User { get; private set; } = null!;
    
    protected Session() { } // For EF
    public Session(Guid userId, string deviceId, DateTime expiresAt)
    {
        UserId = userId;
        DeviceId = deviceId;
        ExpiresAt = expiresAt;
        Token = Guid.NewGuid().ToString("N");
    }
}