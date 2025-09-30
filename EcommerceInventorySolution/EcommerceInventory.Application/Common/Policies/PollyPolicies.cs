using Microsoft.EntityFrameworkCore;
using Polly;

namespace EcommerceInventory.Application.Common.Policies;
public static class PollyPolicies
{
    public static IAsyncPolicy CreateConcurrencyRetryPolicy()
    {
        return Policy
            .Handle<DbUpdateConcurrencyException>()
            .WaitAndRetryAsync(
                retryCount: 3,
                sleepDurationProvider: retryAttempt => TimeSpan.FromMilliseconds(100 * retryAttempt)
            );
    }
}
