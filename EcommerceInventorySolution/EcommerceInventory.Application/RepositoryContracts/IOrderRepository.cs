using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcommerceInventory.Application.RepositoryContracts;
public interface IOrderRepository
{
    Task AddOrderAsync(Domain.Entities.Order order);
    Task<Domain.Entities.Order?> GetOrderByIdWithItemsAsync(Guid orderId);
    Task<IEnumerable<Domain.Entities.Order>> GetOrdersByUserIdAsync(Guid userId);
}
