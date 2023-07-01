using VateraLite.Application.Interfaces;
using VateraLite.Infrastructure.Persistence.Repositories;

namespace VateraLite.Infrastructure.Persistence
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _context;
        private readonly Lazy<ICustomerRepository> _customers;
        private readonly Lazy<IProductRepository> _products;
        private readonly Lazy<OrderRepository> _orders;

        public UnitOfWork(AppDbContext context)
        {
            _context = context;
            _customers = new(() => new CustomerRepository(_context));
            _products= new(() => new ProductRepository(_context));
            _orders = new(() => new OrderRepository(_context));
        }

        public ICustomerRepository Customers => _customers.Value;
        public IProductRepository Products => _products.Value;
        public IOrderRepository Orders => _orders.Value;

        public async Task<int> CompleteAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
