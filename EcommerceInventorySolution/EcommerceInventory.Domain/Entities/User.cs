namespace EcommerceInventory.Domain.Entities;
public class User
{
    public Guid Id { get; private set; } = Guid.NewGuid();
    public string Username { get; private set; }
    public byte[] PasswordHash { get; private set; }
    public byte[] PasswordSalt { get; private set; }

    public List<Session> Sessions { get; private set; } = new();

    public User(string userName, string password)
    {
        Username = userName;
        SetPassword(password);
    }

    public void SetPassword(string password)
    {
        using var hmac = new System.Security.Cryptography.HMACSHA512();
        PasswordSalt = hmac.Key;
        PasswordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
    }

    public bool VerifyPassword(string password)
    {
        using var hmac = new System.Security.Cryptography.HMACSHA512(PasswordSalt);
        var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
        return computedHash.SequenceEqual(PasswordHash);
    }

    public void AddSession(Session session)
    {
        Sessions.Add(session);
    }

    public void RemoveSession(Session session)
    {
        Sessions.Remove(session);
    }

    public void RemoveAllSessions()
    {
        Sessions.Clear();
    }
}