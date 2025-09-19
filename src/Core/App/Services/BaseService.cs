using System.Linq.Expressions;
using Core.App.DTOs.Common;
using Core.App.Repositories.Interfaces;
using Core.App.Services.Interfaces;

namespace Core.App.Services;

public class BaseService<TEntity> : IBaseService<TEntity> where TEntity : class, IBaseEntity
{
    private readonly IBaseRepository<TEntity> _repository;

    public BaseService(IBaseRepository<TEntity> repository)
    {
        _repository = repository;
    }


    public virtual async Task<TEntity> GetByIdAsync(long id, bool withDeleted = false)
    {
        return await _repository.GetByIdAsync(id, withDeleted);
    }

    public async Task<TEntity> GetAsync(Expression<Func<TEntity, bool>> predicate, bool withDeleted = false)
    {
        return await _repository.GetAsync(predicate, withDeleted);
    }

    public async Task<(int, IEnumerable<TEntity>)> LoadAsync(int page = 1, int size = 10,
        Expression<Func<TEntity, bool>> predicate = null)
    {
        return await _repository.LoadAsync(page, size, predicate);
    }

    public async Task<TEntity> AddAsync(TEntity entity)
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

    public virtual async Task<bool> ExistsAsync(long id)
    {
        return await _repository.ExistsAsync(id);
    }

    public async Task<bool> ExistsAsync(Expression<Func<TEntity, bool>> predicate)
    {
        return await _repository.ExistsAsync(predicate);
    }

    public async Task<int> CountAsync(Expression<Func<TEntity, bool>> predicate)
    {
        return await _repository.CountAsync(predicate);
    }
}