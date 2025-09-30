using EcommerceInventory.Application.DTO.OrderDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcommerceInventory.Application.ServiceContracts;
public interface IOrderService
{
    Task<CreateOrderResponseDto> CreateOrderAsync(Guid userId, List<OrderItemDto> items);
}
