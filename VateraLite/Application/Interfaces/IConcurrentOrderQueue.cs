using VateraLite.Application.Dtos;

namespace VateraLite.Application.Interfaces
{
    public interface IConcurrentOrderQueue
    {
        void AddOrder(OrderWithProductsDto orderWithProductsDto);
        Task<OrderWithProductsDto> GetOrderAsync(CancellationToken cancellationToken);
    }
}
