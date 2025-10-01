using EcommerceInventory.Domain.Contracts;

namespace EcommerceInventory.Domain.Entities.Orders;
public class Order
{
    public Guid Id { get; private set; } = Guid.NewGuid();
    public Guid UserId { get; private set; }
    public decimal TotalAmount { get; private set; }
    public string Status { get; private set; } = "Pending"; // Pending, Processing, Completed, Failed
    public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;
    public string DiscountCardType { get; private set; } = string.Empty;
    public decimal FinalAmount { get; private set; }

    public User User { get; private set; } = null!;
    public List<OrderItem> Items { get; private set; } = new();
    private IDiscountStrategy _discountStrategy;

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

    public void SetDiscountStrategy(IDiscountStrategy discountStrategy)
       => _discountStrategy = discountStrategy;

   public void ApplyDiscount()
    {
        if (_discountStrategy != null)
        {
            FinalAmount = _discountStrategy.ApplyDiscount(TotalAmount);
            DiscountCardType = _discountStrategy.Name;
        }
        else
        {
            FinalAmount = TotalAmount;
            DiscountCardType = string.Empty;
        }
    }
    public void MarkAsProcessing() => Status = "Processing";
    public void MarkAsCompleted() => Status = "Completed";
    public void MarkAsFailed() => Status = "Failed";
}
