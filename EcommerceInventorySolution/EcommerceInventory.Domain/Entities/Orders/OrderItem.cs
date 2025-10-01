namespace EcommerceInventory.Domain.Entities.Orders;
public class OrderItem
{
    public Guid Id { get; private set; } = Guid.NewGuid();
    public Guid OrderId { get; private set; }
    public Guid ProductId { get; private set; }
    public int Quantity { get; private set; }
    public decimal Price { get; private set; }
    public decimal TotalPrice => Price * Quantity;


    public Order Order { get; private set; } = null!;
    public Product Product { get; private set; } = null!;

    protected OrderItem() { } // EF Core

    public OrderItem(Order order, Guid productId, int quantity, decimal price)
    {
        Order = order;
        OrderId = order.Id;
        ProductId = productId;
        Quantity = quantity;
        Price = price;
    }
}

