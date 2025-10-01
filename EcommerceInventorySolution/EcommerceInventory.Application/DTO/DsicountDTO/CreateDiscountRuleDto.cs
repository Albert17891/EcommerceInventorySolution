using EcommerceInventory.Domain.Entities.Discounts;

namespace EcommerceInventory.Application.DTO.DsicountDTO;
public record CreateDiscountRuleDto(
    string CardType,
    DiscountType Type,
    decimal? DiscountPercentage,
    decimal? FixedAmount,
    decimal MinimumPurchaseAmount,
    DateTime ValidFrom,
    DateTime ValidTo
);
