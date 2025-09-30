namespace EcommerceInventory.Application.DTO.ProductDTO;
public record PurchaseProductResponseDto(
    Guid OrderId,
    bool Success,
    string Message
);

