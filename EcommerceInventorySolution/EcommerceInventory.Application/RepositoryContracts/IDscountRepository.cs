using EcommerceInventory.Domain.Entities.Discounts;

namespace EcommerceInventory.Application.RepositoryContracts;
public interface IDiscountRepository
{
    Task AddDiscountRuelAsync(DiscountRule discountRule);
    void UpdateDiscountRule(DiscountRule discountRule);
    Task<DiscountRule?> GetByIdAsync(Guid id);
    Task<IEnumerable<DiscountRule>> GetAllAsync();
    Task<DiscountRule> GetActiveDiscountByCardTypeAsync(string cardType);
}
