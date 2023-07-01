using VateraLite.Application.Interfaces;
using VateraLite.Domain.Entities;

namespace VateraLite.Infrastructure.Persistence.Repositories
{
    public class OrderRepository : GenericRepository<Order, Guid>, IOrderRepository
    {
        public OrderRepository(AppDbContext context) : base(context)
        {

        }

        public AppDbContext NorthwindContext
        {
            get { return _context as AppDbContext; }
        }
    }
}
