namespace EcommerceInventory.Domain.Contracts;
public interface IDiscountStrategy
{
    string Name { get; }
    decimal ApplyDiscount(decimal totalAmount);
}
