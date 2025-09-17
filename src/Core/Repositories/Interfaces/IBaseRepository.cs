using System.Linq.Expressions;
using Core.App.DTOs.Common;
using Core.DTOs.Common;
using Microsoft.EntityFrameworkCore.Storage;

namespace Core.Repositories.Interfaces;

public interface IBaseRepository<T> where T : BaseEntity
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