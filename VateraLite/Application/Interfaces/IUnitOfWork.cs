namespace VateraLite.Application.Interfaces
{
    public interface IUnitOfWork
    {
        public ICustomerRepository Customers { get; }
        public IOrderRepository Orders { get; }
        public IProductRepository Products { get; }

        Task<int> CompleteAsync();
    }
}
