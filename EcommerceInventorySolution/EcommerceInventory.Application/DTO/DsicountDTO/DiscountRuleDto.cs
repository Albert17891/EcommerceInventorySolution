using EcommerceInventory.Domain.Entities.Discounts;

namespace EcommerceInventory.Application.DTO.DsicountDTO;
public record DiscountRuleDto(
    Guid Id,
    string CardType,
    DiscountType Type,
    decimal? DiscountPercentage,
    decimal? FixedAmount,
    decimal MinimumPurchaseAmount,
    DateTime ValidFrom,
    DateTime ValidTo,
    bool Active
);
