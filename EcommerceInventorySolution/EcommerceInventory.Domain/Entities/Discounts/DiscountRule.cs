namespace EcommerceInventory.Domain.Entities.Discounts;
public class DiscountRule
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string CardType { get; set; }
    public DiscountType Type { get; private set; }
    public decimal? DiscountPercentage { get; set; }
    public decimal? MinimumPurchaseAmount { get; set; }
    public decimal? FixedAmount { get; private set; }
    public DateTime ValidFrom { get; set; }
    public DateTime ValidTo { get; set; }
    public bool Active { get; private set; } = true;
    // Constructor for EF
    protected DiscountRule() { }

    public DiscountRule(string cardType, DiscountType type, decimal? discountPercentage, decimal? minimumPurchaseAmount, decimal? fixedAmount, DateTime validFrom, DateTime validTo, bool active)
    {
        CardType = cardType;
        Type = type;
        DiscountPercentage = discountPercentage;
        MinimumPurchaseAmount = minimumPurchaseAmount;
        FixedAmount = fixedAmount;
        ValidFrom = validFrom;
        ValidTo = validTo;
        Active = active;
    }

    public bool IsValid(decimal totalAmount)
    {
        var now = DateTime.UtcNow;
        return Active &&
            now >= ValidFrom &&
            now <= ValidTo &&
            totalAmount >= MinimumPurchaseAmount;
    }
}
