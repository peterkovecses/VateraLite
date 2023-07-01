using VateraLite.Application.Interfaces;
using VateraLite.Domain.Entities;

namespace VateraLite.Infrastructure.Persistence.Repositories
{
    public class CustomerRepository : GenericRepository<Customer, int>, ICustomerRepository
    {
        public CustomerRepository(AppDbContext context) : base(context)
        {
        }

        public AppDbContext NorthwindContext
        {
            get { return _context as AppDbContext; }
        }
    }
}
