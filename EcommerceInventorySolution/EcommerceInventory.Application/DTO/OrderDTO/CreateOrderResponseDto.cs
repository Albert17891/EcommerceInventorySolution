namespace EcommerceInventory.Application.DTO.OrderDTO;
public record CreateOrderResponseDto(Guid OrderId, bool Success, string Message);
