using EcommerceInventory.Application.ServiceContracts;
using System.Threading.Channels;

namespace EcommerceInventory.Application.Services;
public class BackgroundTaskQueue : IBackgroundTaskQueue
{
    private readonly Channel<Func<IServiceProvider, CancellationToken, Task>> _queue;

    public BackgroundTaskQueue(int capacity = 100)
    {
        var options = new BoundedChannelOptions(capacity)
        {
            FullMode = BoundedChannelFullMode.Wait
        };

        _queue = Channel.CreateBounded<Func<IServiceProvider, CancellationToken, Task>>(options);
    }
    public async IAsyncEnumerable<Func<IServiceProvider, CancellationToken, Task>> DequeueAsync(CancellationToken cancellationToken)
    {
        while (await _queue.Reader.WaitToReadAsync(cancellationToken))
        {
            while (_queue.Reader.TryRead(out var workItem))
            {
                yield return workItem;
            }
        }
    }

    public void QueueBackgroundWorkItem(Func<IServiceProvider, CancellationToken, Task> workItem)
    {
        if (workItem is null)
        {
            throw new ArgumentNullException(nameof(workItem));
        }

        _queue.Writer.TryWrite(workItem);
    }

}
