using System.Linq.Expressions;
using Domain.Entities.Common;
using Microsoft.EntityFrameworkCore.Storage;

namespace Domain.Interfaces.Repositories;

public interface IBaseRepository<T> where T : IBaseEntity
{
    Task<T> GetByIdAsync(long id);
    Task<T> GetAsync(Expression<Func<T, bool>> predicate = null);
    Task<IEnumerable<T>> LoadAsync(int page, int size, Expression<Func<T, bool>> predicate = null);
    Task<T> AddAsync(T entity);
    Task<T> UpdateAsync(T entity);
    Task<bool> DeleteAsync(long id);
    Task<bool> ExistsAsync(long id);
    Task<bool> ExistsAsync(Expression<Func<T, bool>> predicate = null);
    Task<int> CountAsync(Expression<Func<T, bool>> predicate = null);
    
    IDbContextTransaction BeginTransaction();
    void RefreshEntity(T entity);
}