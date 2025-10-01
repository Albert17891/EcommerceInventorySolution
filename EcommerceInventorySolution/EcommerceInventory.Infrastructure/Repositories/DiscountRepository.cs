using EcommerceInventory.Application.RepositoryContracts;
using EcommerceInventory.Domain.Entities.Discounts;
using EcommerceInventory.Infrastructure.DataContext;
using Microsoft.EntityFrameworkCore;

namespace EcommerceInventory.Infrastructure.Repositories;
public class DiscountRepository : IDiscountRepository
{
    private readonly AppDbContext _context;

    public DiscountRepository(AppDbContext context)
    {
        _context = context;
    }
    public async Task AddDiscountRuelAsync(DiscountRule discountRule)
    {
        await _context.DiscountRules.AddAsync(discountRule);
    }

    public async Task<DiscountRule> GetActiveDiscountByCardTypeAsync(string cardType)
    {
        return await _context.DiscountRules.FirstOrDefaultAsync(x => x.CardType == cardType && x.Active);
    }

    public async Task<IEnumerable<DiscountRule>> GetAllAsync()
    {
        return await _context.DiscountRules.ToListAsync();
    }

    public async Task<DiscountRule?> GetByIdAsync(Guid id)
    {
        return await _context.DiscountRules.FirstOrDefaultAsync(x => x.Id == id);
    }

    public void UpdateDiscountRule(DiscountRule discountRule)
    {
        _context.DiscountRules.Update(discountRule);
    }
}
