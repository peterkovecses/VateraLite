namespace VateraLite.Application.Interfaces
{
    public interface IGenericService<TEntity, TId> where TEntity : class
 
    {
        Task<List<TEntity>> GetAsync(CancellationToken token = default);
        Task<TEntity?> FindByIdAsync(TId id, CancellationToken token = default);
        Task<TEntity> CreateAsync(TEntity obj, CancellationToken token = default);
        Task<TEntity> UpdateAsync(TEntity obj, CancellationToken token = default);
        Task DeleteAsync(TId id, CancellationToken token = default);
    }
}
