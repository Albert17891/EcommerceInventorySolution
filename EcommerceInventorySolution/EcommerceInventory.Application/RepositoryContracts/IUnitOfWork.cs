namespace EcommerceInventory.Application.RepositoryContracts;

public interface IUnitOfWOrk: IDisposable
{
    IUserRepository Users { get; }
    Task<int> CompleteAsync();
}
