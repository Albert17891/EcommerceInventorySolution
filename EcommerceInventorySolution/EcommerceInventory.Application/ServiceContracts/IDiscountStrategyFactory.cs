using EcommerceInventory.Domain.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcommerceInventory.Application.ServiceContracts;
public interface IDiscountStrategyFactory
{
    Task<IDiscountStrategy> CreateAsync(string cardType, decimal orderAmount);
}
