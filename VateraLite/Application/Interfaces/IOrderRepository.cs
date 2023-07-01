using VateraLite.Domain.Entities;

namespace VateraLite.Application.Interfaces
{
    public interface IOrderRepository : IGenericRepository<Order, Guid>
    {
    }
}
