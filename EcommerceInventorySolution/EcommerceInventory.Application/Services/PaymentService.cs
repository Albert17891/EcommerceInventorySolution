using EcommerceInventory.Application.ServiceContracts;

namespace EcommerceInventory.Application.Services;
public class PaymentService : IPaymentService
{
    public async Task<bool> ProcessPaymentAsync(decimal amount)
    {
        // Simulate payment processing time
        await Task.Delay(TimeSpan.FromMinutes(2));

        // Simulate 90% success rate
        return Random.Shared.NextDouble() > 0.1;
    }
}
