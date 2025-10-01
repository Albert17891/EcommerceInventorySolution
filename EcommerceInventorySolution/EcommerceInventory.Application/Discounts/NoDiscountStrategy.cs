using EcommerceInventory.Domain.Contracts;

namespace EcommerceInventory.Application.Discounts;
public class NoDiscountStrategy : IDiscountStrategy
{
    public string Name => "No Discount";
    public decimal ApplyDiscount(decimal totalAmount)
    {
        return totalAmount;
    }
}
