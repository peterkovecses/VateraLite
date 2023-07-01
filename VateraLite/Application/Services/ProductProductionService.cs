using VateraLite.Application.Interfaces;

namespace VateraLite.Application.Services
{
    public class ProductProductionService : BackgroundService
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;

        public ProductProductionService(IServiceScopeFactory serviceScopeFactory)
        {
            _serviceScopeFactory = serviceScopeFactory;
        }

        protected override async Task ExecuteAsync(CancellationToken token)
        {
            using var scope = _serviceScopeFactory.CreateScope();
            var stockService = scope.ServiceProvider.GetRequiredService<IStockService>();

            while (!token.IsCancellationRequested)
            {
                var stockQuantity = await stockService.GetStockQuantityAsync(token);
                if (stockQuantity < 100)
                {
                    await stockService.UploadAsync(token);
                }

                await Task.Delay(TimeSpan.FromSeconds(10), token);
            }
        }
    }
}
