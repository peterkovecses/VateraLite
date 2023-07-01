using VateraLite.Application.Models;

namespace VateraLite.Application.Interfaces
{
    public interface IStockService
    {
        Task<int> GetStockQuantityAsync(CancellationToken token = default);
        Task UploadAsync(CancellationToken token = default);
        Task<OrderResult> ReserveAsync(int quantity, int customerId, CancellationToken token = default);
    }
}
