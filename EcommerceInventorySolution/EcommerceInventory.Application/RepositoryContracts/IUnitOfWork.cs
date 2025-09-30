namespace EcommerceInventory.Application.RepositoryContracts;

public interface IUnitOfWork: IDisposable
{
    IUserRepository Users { get; }
    IProductRepository Products { get; }
    IOrderRepository Orders { get; }
    IOutboxRepository OutboxMessages { get; }
    Task<int> CompleteAsync();
}
