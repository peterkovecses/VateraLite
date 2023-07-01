using VateraLite.Application.Dtos;

namespace VateraLite.Application.Interfaces
{
    public interface IOrderService : IGenericService<OrderDto, Guid>
    {
        Task<bool> CanOrder(int customerId, int quantity, CancellationToken token = default);
        bool ValidateAmount(int quantity);
        Task<OrderDto> CreateAsync(OrderDto orderDto, IEnumerable<ProductDto> productDtos, CancellationToken token = default);
    }
}
