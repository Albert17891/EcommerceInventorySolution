using EcommerceInventory.Domain.Contracts;
using EcommerceInventory.Domain.Entities.Discounts;

namespace EcommerceInventory.Application.Discounts;
public class ConfigurableDiscountStrategy : IDiscountStrategy
{
    private readonly DiscountRule _discountRule;

    public ConfigurableDiscountStrategy(DiscountRule discountRule)
    {
        _discountRule = discountRule;
    }

    public string Name => _discountRule.CardType;

    public decimal ApplyDiscount(decimal totalAmount)
    {
        if (_discountRule == null || !_discountRule.IsValid(totalAmount))
            return totalAmount;

        return _discountRule.Type switch
        {
            DiscountType.Percentage when _discountRule.DiscountPercentage.HasValue =>
                totalAmount - (totalAmount * _discountRule.DiscountPercentage.Value / 100),

            DiscountType.Fixed when _discountRule.FixedAmount.HasValue =>
            totalAmount - _discountRule.FixedAmount.Value,

            _ => totalAmount
        };
    }
}
