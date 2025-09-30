using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcommerceInventory.Application.ServiceContracts;
public interface IBackgroundTaskQueue
{
    void QueueBackgroundWorkItem(Func<IServiceProvider,CancellationToken, Task> workItem);
    IAsyncEnumerable<Func<IServiceProvider, CancellationToken, Task>> DequeueAsync(CancellationToken cancellationToken);
}
