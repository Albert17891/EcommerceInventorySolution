using EcommerceInventory.Application.DTO.ProductDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcommerceInventory.Application.ServiceContracts;
public interface IProductService
{
    Task<ProductResponseDto> CreateProductAsync(CreateProductRequestDto createProductRequestDto);
    Task<ProductResponseDto?> UpdateProductAsync(Guid productId, UpdateProductRequestDto updateProductRequestDto);
    Task<ProductResponseDto?> GetProductByIdAsync(Guid productId);
    Task<IEnumerable<ProductResponseDto>> GetAllProductsAsync();
}
    

