namespace EcommerceInventory.Application.DTO.ProductDTO;
public record PurchaseProductRequestDto(
    Guid ProductId,
    int Quantity
);

