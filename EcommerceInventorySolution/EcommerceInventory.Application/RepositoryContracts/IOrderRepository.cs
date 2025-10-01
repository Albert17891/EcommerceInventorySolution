using EcommerceInventory.Domain.Entities.Orders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcommerceInventory.Application.RepositoryContracts;
public interface IOrderRepository
{
    Task AddOrderAsync(Order order);
    Task<Order?> GetOrderByIdWithItemsAsync(Guid orderId);
    Task<IEnumerable<Order>> GetOrdersByUserIdAsync(Guid userId);
}
