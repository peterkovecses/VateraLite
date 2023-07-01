using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using VateraLite.Application.Interfaces;

namespace VateraLite.Infrastructure.Persistence.Repositories
{
    public abstract class GenericRepository<TEntity, TId> : IGenericRepository<TEntity, TId> where TEntity : class
    {
        protected readonly DbContext _context;

        public GenericRepository(DbContext context)
        {
            _context = context;
        }

        public async Task<List<TEntity>> GetAsync(Expression<Func<TEntity, bool>>? predicate = default, int? take = null, CancellationToken token = default)
        {
            var query = predicate == default ? _context.Set<TEntity>() : _context.Set<TEntity>().Where(predicate);
            if (take != null)
            {
                query = query.Take(take.Value);
            }
            return await query.ToListAsync(token);
        }


        public virtual async Task<TEntity?> FindByIdAsync(TId id, CancellationToken token = default)
        {
            return await _context.Set<TEntity>().FindAsync(new object?[] { id }, cancellationToken: token);
        }

        public async Task AddAsync(TEntity entity, CancellationToken token = default)
        {
            await _context.Set<TEntity>().AddAsync(entity, token);
        }

        public void Remove(TEntity entity)
        {
            _context.Set<TEntity>().Remove(entity);
        }
    }
}
