using EcommerceInventory.Application.RepositoryContracts;
using EcommerceInventory.Infrastructure.DataContext;

namespace EcommerceInventory.Infrastructure.Repositories;
public class UnitOfWork : IUnitOfWork
{
    private readonly AppDbContext _context;
    private readonly IUserRepository _userRepository;
    private readonly IProductRepository _productRepository;
    private readonly IOrderRepository _orderRepository;
    private readonly IOutboxRepository _outboxReposiotry;

    public UnitOfWork(AppDbContext context,
        IUserRepository repository,
        IProductRepository productRepository,
        IOrderRepository orderRepository,
        IOutboxRepository outboxRepository)
    {
        _context = context;
        _userRepository = repository;
        _productRepository = productRepository;
        _orderRepository = orderRepository;
        _outboxReposiotry= outboxRepository;

    }
    public IUserRepository Users => _userRepository;

    public IProductRepository Products => _productRepository;

    public IOrderRepository Orders => _orderRepository;

    public IOutboxRepository OutboxMessages => _outboxReposiotry;

    public async Task<int> CompleteAsync()
    {
        return await _context.SaveChangesAsync();
    }

    public void DetachEntity<T>(T entity) where T : class
    {
        if (entity is not null)
        {
            var entry = _context.Entry(entity);

            if (entry != null)
            {
                entry.State = Microsoft.EntityFrameworkCore.EntityState.Detached;
            }
        }
    }

    public void Dispose()
    {
        _context.Dispose();
    }
}
