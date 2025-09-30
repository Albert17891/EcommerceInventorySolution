namespace EcommerceInventory.Application.DTO.ProductDTO;
public record UpdateProductRequestDto(
    string? Name = null,
    decimal? Price = null,
    int? Stock = null
);