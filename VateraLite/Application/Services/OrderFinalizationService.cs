using Microsoft.AspNetCore.SignalR;
using VateraLite.Application.Interfaces;

namespace VateraLite.Application.Services
{
    public class OrderFinalizationService : BackgroundService
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly ILogger<OrderFinalizationService> _logger;
        private readonly IHubContext<OrderHub> _hubContext;

        public OrderFinalizationService(IServiceScopeFactory serviceScopeFactory, ILogger<OrderFinalizationService> logger, IHubContext<OrderHub> hubContext)
        {
            _serviceScopeFactory = serviceScopeFactory;
            _logger = logger;
            _hubContext = hubContext;
        }

        protected override async Task ExecuteAsync(CancellationToken token)
        {
            while (!token.IsCancellationRequested)
            {
                using (var scope = _serviceScopeFactory.CreateScope())
                {
                    var queue = scope.ServiceProvider.GetRequiredService<IConcurrentOrderQueue>();
                    var order = await queue.GetOrderAsync(token);
                    var orderService = scope.ServiceProvider.GetRequiredService<IOrderService>();

                    try
                    {
                        var createdOrder = await orderService.CreateAsync(order.Order, order.Products, token);

                        _logger.LogInformation($"Stock reduction by {order.Products.Count()} units");
                        await _hubContext.Clients.User(order.Order.CustomerId.ToString()).SendAsync("OrderStatus", $"Successful order: {order.Order.Id}");
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, $"Error finalizing order with id {order.Order.Id}");
                        await _hubContext.Clients.User(order.Order.CustomerId.ToString()).SendAsync("OrderStatus", $"Order failed: {order.Order.Id}");
                    }
                }

                await Task.Delay(TimeSpan.FromSeconds(10), token);
            }
        }
    }
}
