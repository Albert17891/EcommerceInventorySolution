namespace EcommerceInventory.Application.ServiceContracts;
public interface IPaymentService
{
    Task<bool> ProcessPaymentAsync(decimal amount);
}
