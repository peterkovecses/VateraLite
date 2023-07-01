using VateraLite.Application.Dtos;
using VateraLite.Application.Interfaces;
using VateraLite.Application.Models;

namespace VateraLite.Application.Services
{
    public class StockService : IStockService
    {
        private readonly IProductService _productService;
        private readonly IOrderService _orderService;
        private readonly IConcurrentOrderQueue _queue;
        private readonly ILogger<StockService> _logger;

        public StockService(IProductService productService, IOrderService orderService, IConcurrentOrderQueue queue, ILogger<StockService> logger)
        {
            _productService = productService;
            _orderService = orderService;
            _queue = queue;
            _logger = logger;
        }

        public async Task<int> GetStockQuantityAsync(CancellationToken token = default)
        {
            return (await _productService.GetAsync(token)).Where(product => product.OrderId == null).Count();
        }

        public async Task UploadAsync(CancellationToken token = default)
        {
            var random = new Random();
            var productIncrease = random.Next(1, 11);
            for (int i = 0; i < productIncrease; i++)
            {
                await _productService.CreateAsync(new ProductDto { Id = new Guid() }, token);
            }
            _logger.LogInformation($"Stock increase by {productIncrease} units");
        }

        public async Task<OrderResult> ReserveAsync(int quantity, int customerId, CancellationToken token = default)
        {
            if (_orderService.ValidateAmount(quantity))
            {
                return new OrderResult { Message = "Invalid amount" };
            }

            var products = await _productService.GetAsync(quantity, token);
            if (products.Count < quantity)
            {
                return new OrderResult { Message = "The ordered quantity is not in stock" };
            }

            if (!await _orderService.CanOrder(customerId, quantity, token))
            {
                return new OrderResult { Message = "The given customer can no longer order such a large quantity of this product." };
            }

            var orderId = Guid.NewGuid();
            products.ForEach(product => product.OrderId = orderId);

            var orderDto = new OrderDto
            {
                Id = orderId,
                CustomerId = customerId,
                Time = DateTime.UtcNow,
            };

            var order = new OrderWithProductsDto
            {
                Order = orderDto,
                Products = products
            };

            _queue.AddOrder(order);

            return new OrderResult { Message = "Order in progress" };
        }
    }
}
