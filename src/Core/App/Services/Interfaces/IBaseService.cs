using System.Linq.Expressions;
using Core.App.DTOs.Common;
using Microsoft.EntityFrameworkCore.Storage;

namespace Core.App.Services.Interfaces;

public interface IBaseService<TEntity> where TEntity : class, IBaseEntity
{
    Task<TEntity> GetByIdAsync(long id, bool asTracking = false, bool withDeleted = false);
    Task<TEntity> GetAsync(Expression<Func<TEntity, bool>> predicate = null, bool asTracking = false, bool withDeleted = false);

    // Task<IEnumerable<TEntity>> LoadAsync(int page, int size, Expression<Func<TEntity, bool>> predicate = null);
    Task<(int, IEnumerable<TEntity>)> LoadAsync(int page = 1, int size = 10,
        Expression<Func<TEntity, bool>> predicate = null);

    Task<TEntity> AddAsync(TEntity entity);
    Task<TEntity> UpdateAsync(TEntity entity);
    Task<bool> SoftDeleteAsync(long id);
    Task<bool> PermanentDeleteAsync(long id);
    Task<bool> ExistsAsync(long id, bool withDeleted = false);
    Task<bool> ExistsAsync(Expression<Func<TEntity, bool>> predicate = null, bool withDeleted = false);
    Task<int> CountAsync(Expression<Func<TEntity, bool>> predicate = null);
    
    
    Task<TEntity> RestoreAsync(long id);
    Task<TEntity> ToggleStatusAsync(long id);
    
    Task<IDbContextTransaction> BeginTransactionAsync();
    Task RefreshEntity(TEntity entity);
    
}