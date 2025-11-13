using System.Linq.Expressions;
using Core.App.DTOs.Common;
using Core.App.Repositories.Interfaces;
using Core.App.Services.Interfaces;
using Microsoft.EntityFrameworkCore.Storage;

namespace Core.App.Services;

public class BaseService<TEntity> : IBaseService<TEntity> where TEntity : class, IBaseEntity
{
    private readonly IBaseRepository<TEntity> _repository;

    public BaseService(IBaseRepository<TEntity> repository)
    {
        _repository = repository;
    }


    public virtual async Task<TEntity> GetByIdAsync(long id, bool asTracking = false, bool withDeleted = false)
    {
        return await _repository.GetByIdAsync(id, asTracking, withDeleted);
    }

    public virtual async Task<TEntity> GetAsync(Expression<Func<TEntity, bool>> predicate, bool asTracking = false, bool withDeleted = false)
    {
        return await _repository.GetAsync(predicate, asTracking, withDeleted);
    }

    public async Task<(int, IEnumerable<TEntity>)> LoadAsync(int page = 1, int size = 10,
        Expression<Func<TEntity, bool>> predicate = null)
    {
        return await _repository.LoadAsync(page, size, predicate);
    }

    public virtual async Task<TEntity> AddAsync(TEntity entity)
    {
        return await _repository.AddAsync(entity);
    }

    public virtual async Task<TEntity> UpdateAsync(TEntity entity)
    {
        return await _repository.UpdateAsync(entity);
    }

    public async Task<bool> SoftDeleteAsync(long id)
    {
        return await _repository.DeleteAsync(id);
    }

    public Task<bool> PermanentDeleteAsync(long id)
    {
        return _repository.PermanentDeleteAsync(id);
    }

    public virtual async Task<bool> ExistsAsync(long id, bool withDeleted = false)
    {
        return await _repository.ExistsAsync(id, withDeleted);
    }

    public async Task<bool> ExistsAsync(Expression<Func<TEntity, bool>> predicate, bool withDeleted = false)
    {
        return await _repository.ExistsAsync(predicate, withDeleted);
    }

    public async Task<int> CountAsync(Expression<Func<TEntity, bool>> predicate)
    {
        return await _repository.CountAsync(predicate);
    }
    
    public virtual async Task<TEntity> RestoreAsync(long id)
    {
        return await _repository.RestoreAsync(id);
    }

    public virtual async Task<TEntity> ToggleStatusAsync(long id)
    {
        return await _repository.ToggleStatusAsync(id);
    }

    public async Task<IEnumerable<TEntity>> ExecuteSqlQueryListAsync(string sql, params object[] parameters)
    {
        return await _repository.ExecuteSqlQueryListAsync(sql, parameters);
    }

    public async Task<int> SaveChangesAsync()
    {
        return await _repository.SaveChangesAsync();
    }

    public async Task<IDbContextTransaction> BeginTransactionAsync()
    {
        return await _repository.BeginTransactionAsync();
    }

    public async Task RefreshEntity(TEntity entity)
    {
        await _repository.RefreshEntity(entity);
    }
}