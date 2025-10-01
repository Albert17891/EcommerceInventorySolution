namespace EcommerceInventory.Domain.Entities.Orders;
public class OrderCompletedEvent
{
    public Guid OrderId { get; set; }
    public Guid UserId { get; set; }
    public decimal TotalAmount { get; set; }
    public DateTime CompletedAt { get; set; }
}
