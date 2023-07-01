using VateraLite.Application.Dtos;

namespace VateraLite.Application.Interfaces
{
    public interface IProductService : IGenericService<ProductDto, Guid>
    {
        Task<List<ProductDto>> GetAsync(int quantity = default, CancellationToken token = default);
    }
}
