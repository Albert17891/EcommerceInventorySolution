namespace EcommerceInventory.Application.DTO.ProductDTO;
public record ProductResponseDto(
    Guid Id,
    string Name,
    decimal Price,
    int Stock
);

