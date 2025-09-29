namespace EcommerceInventory.Domain.Entities;

public class Session
{
    public Guid Id { get; private set; } = Guid.NewGuid();
    public Guid UserId { get; private set; }
    public string DeviceId { get; private set; }
    public DateTime ExpiresAt { get; private set; }

    public Session(Guid userId, string deviceId, DateTime expiresAt)
    {
        UserId = userId;
        DeviceId = deviceId;
        ExpiresAt = expiresAt;
    }
}