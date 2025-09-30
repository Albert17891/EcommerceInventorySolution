namespace EcommerceInventory.Application.ServiceContracts;
public interface IEventPublisher
{
    Task PublishAsync<T>(T @event) where T : class;
}