using EcommerceInventory.Domain.Entities;

namespace EcommerceInventory.Application.RepositoryContracts;

public interface IProductRepository
{
    Task AddProductAsync(Product product);
    Task<Product?> GetProductByIdAsync(Guid productId);
    void UpdateProduct(Product product);
    void DeleteProduct(Product product);
    Task<IEnumerable<Product>> GetAllProductsAsync();
}
