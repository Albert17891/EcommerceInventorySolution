namespace EcommerceInventory.Domain.Entities;
public class Order
{
    public Guid Id { get; private set; } = Guid.NewGuid();
    public Guid UserId { get; private set; }
    public decimal TotalAmount { get; private set; }
    public string Status { get; private set; } = "Pending"; // Pending, Processing, Completed, Failed
    public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;

    public User User { get; private set; } = null!;
    public List<OrderItem> Items { get; private set; } = new();

    // Constructor for EF
    protected Order() { }

    public Order(Guid userId)
    {
        UserId = userId;
    }

    public void AddItem(Guid productId, int quantity, decimal price)
    {
        var item = new OrderItem(this, productId, quantity, price);
        Items.Add(item);
        TotalAmount += item.TotalPrice;
    }

    public void MarkAsProcessing() => Status = "Processing";
    public void MarkAsCompleted() => Status = "Completed";
    public void MarkAsFailed() => Status = "Failed";
}
