namespace EcommerceInventory.Application.DTO.ProductDTO;
public record CreateProductRequestDto(
    string Name,
    decimal Price,
    int Stock
);

