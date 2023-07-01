using VateraLite.Domain.Entities;

namespace VateraLite.Application.Interfaces
{
    public interface IProductRepository : IGenericRepository<Product, Guid>
    {
    }
}
