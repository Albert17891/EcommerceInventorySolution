using EcommerceInventory.Application.RepositoryContracts;
using EcommerceInventory.Infrastructure.DataContext;

namespace EcommerceInventory.Infrastructure.Repositories;
public class UnitOfWork : IUnitOfWOrk
{
    private readonly AppDbContext _context;
    private readonly IUserRepository _userRepository;

    public UnitOfWork(AppDbContext context, IUserRepository repository)
    {
        _context = context;
        _userRepository = repository;

    }
    public IUserRepository Users => _userRepository;

    public async Task<int> CompleteAsync()
    {
        return await _context.SaveChangesAsync();
    }

    public void Dispose()
    {
        _context.Dispose();
    }
}
