using EcommerceInventory.Application.DTO.OrderDTO;

namespace EcommerceInventory.Application.ServiceContracts;
public interface IOrderService
{
    Task<CreateOrderResponseDto> CreateOrderAsync(Guid userId, List<OrderItemDto> items, string discountCardType = default);
    Task ProcessOrderAsync(Guid id);
}
