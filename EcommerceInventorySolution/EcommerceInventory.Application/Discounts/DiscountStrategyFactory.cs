using EcommerceInventory.Application.RepositoryContracts;
using EcommerceInventory.Application.ServiceContracts;
using EcommerceInventory.Domain.Contracts;

namespace EcommerceInventory.Application.Discounts;

public class DiscountStrategyFactory:IDiscountStrategyFactory
{
    private readonly IDiscountRepository _discountRepository;

    public DiscountStrategyFactory(IDiscountRepository discountRepository)
    {
        _discountRepository = discountRepository;
    }

    // Add this parameterless constructor for testing
    public DiscountStrategyFactory()
    {
    }

    public async Task<IDiscountStrategy> CreateAsync(string? cardType, decimal orderAmount)
    {
        if (string.IsNullOrWhiteSpace(cardType))
            return new NoDiscountStrategy();

        var discountRule = await _discountRepository.GetActiveDiscountByCardTypeAsync(cardType);

        if (discountRule == null || !discountRule.IsValid(orderAmount))
            return new NoDiscountStrategy();

        return new ConfigurableDiscountStrategy(discountRule);
    }
}
