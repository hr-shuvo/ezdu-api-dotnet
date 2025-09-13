using System.Linq.Expressions;
using Domain.Entities.Common;

namespace Core.Services.Interfaces;

public interface IBaseService<TEntity> where TEntity : IBaseEntity
{
    Task<TEntity> GetByIdAsync(long id);
    Task<TEntity> GetAsync(Expression<Func<TEntity, bool>> predicate = null);
    Task<IEnumerable<TEntity>> LoadAsync(int page, int size, Expression<Func<TEntity, bool>> predicate = null);
    Task<TEntity> AddAsync(TEntity entity);
    Task<TEntity> UpdateAsync(TEntity entity);
    Task<bool> DeleteAsync(long id);
    Task<bool> ExistsAsync(long id);
    Task<bool> ExistsAsync(Expression<Func<TEntity, bool>> predicate = null);
    Task<int> CountAsync(Expression<Func<TEntity, bool>> predicate = null);
}