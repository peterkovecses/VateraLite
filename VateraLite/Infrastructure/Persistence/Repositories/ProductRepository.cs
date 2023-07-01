using VateraLite.Application.Interfaces;
using VateraLite.Domain.Entities;

namespace VateraLite.Infrastructure.Persistence.Repositories
{
    public class ProductRepository : GenericRepository<Product, Guid>, IProductRepository
    {
        public ProductRepository(AppDbContext context) : base(context)
        {
        }

        public AppDbContext NorthwindContext
        {
            get { return _context as AppDbContext; }
        }
    }
}
