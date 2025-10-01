using EcommerceInventory.Application.DTO.DsicountDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcommerceInventory.Application.ServiceContracts;
public interface IDiscountService
{
    Task AddDiscountRuleAsync(CreateDiscountRuleDto createDiscountRule);
    Task UpdateDiscountRuleAsync(UpdateDiscountRuleDto updateDiscountRule);
    Task<DiscountRuleDto?> GetByIdAsync(Guid id);
    Task<IEnumerable<DiscountRuleDto>> GetAllAsync();
}
