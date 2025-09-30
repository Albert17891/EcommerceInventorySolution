using EcommerceInventory.Application.RepositoryContracts;
using EcommerceInventory.Domain.Entities;
using EcommerceInventory.Infrastructure.DataContext;
using Microsoft.EntityFrameworkCore;

namespace EcommerceInventory.Infrastructure.Repositories;
public class ProductRepository : IProductRepository
{
    private readonly AppDbContext _dbContext;

    public ProductRepository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task AddProductAsync(Product product)
    {
        await _dbContext.Products.AddAsync(product);
    }

    public void DeleteProduct(Product product)
    {
        _dbContext.Products.Remove(product);
    }

    public async Task<IEnumerable<Product>> GetAllProductsAsync()
        => await _dbContext.Products.ToListAsync();

    public async Task<Product?> GetProductByIdAsync(Guid productId)
          => await _dbContext.Products.AsNoTracking().FirstOrDefaultAsync(p => p.Id == productId);

    public void UpdateProduct(Product product)
    {
        _dbContext.Products.Update(product);
    }
}
