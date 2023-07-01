using System.Linq.Expressions;

namespace VateraLite.Application.Interfaces
{
    public interface IGenericRepository<TEntity, TId> where TEntity : class
    {
        Task<List<TEntity>> GetAsync(Expression<Func<TEntity, bool>>? predicate = default, int? take = null, CancellationToken token = default);
        Task<TEntity?> FindByIdAsync(TId id, CancellationToken token = default);
        Task AddAsync(TEntity entity, CancellationToken token = default);
        void Remove(TEntity entity);
    }
}
