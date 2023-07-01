using System.Collections.Concurrent;
using VateraLite.Application.Dtos;
using VateraLite.Application.Interfaces;

namespace VateraLite.Application.Services
{
    public class ConcurrentOrderQueue : IConcurrentOrderQueue
    {
        private readonly ConcurrentQueue<OrderWithProductsDto> _orderQueue = new();
        private readonly SemaphoreSlim _semaphore = new(0);

        public void AddOrder(OrderWithProductsDto order)
        {
            _orderQueue.Enqueue(order);
            _semaphore.Release();
        }

        public async Task<OrderWithProductsDto> GetOrderAsync(CancellationToken cancellationToken)
        {
            await _semaphore.WaitAsync(cancellationToken);
            _orderQueue.TryDequeue(out var order);

            return order!;
        }
    }
}
