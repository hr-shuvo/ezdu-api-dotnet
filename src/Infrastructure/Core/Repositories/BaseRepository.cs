using System.Linq.Expressions;
using Core.App.DTOs.Common;
using Core.App.Models;
using Core.App.Repositories.Interfaces;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace Infrastructure.Core.Repositories;

public class BaseRepository<T> : IBaseRepository<T> where T : class, IBaseEntity
{
    private readonly AppDbContext _context;
    protected DbSet<T> DbSet;

    public BaseRepository(AppDbContext context)
    {
        _context = context;
        DbSet = context.Set<T>();
    }


    public async Task<T> GetByIdAsync(long id, bool asTracking = false, bool withDeleted = false)
    {
        var query = asTracking ? DbSet.AsQueryable() : DbSet.AsNoTracking();

        if (!withDeleted)
            query = query.Where(x => x.Status != Status.Deleted);

        query = query.Where(x => x.Id == id);

        return await query.FirstOrDefaultAsync();
    }

    public async Task<T> GetAsync(Expression<Func<T, bool>> predicate = null, bool asTracking = false,
        bool withDeleted = false)
    {
        var query = asTracking ? DbSet.AsQueryable() : DbSet.AsNoTracking();

        if (!withDeleted)
            query = query.Where(x => x.Status != Status.Deleted);

        if (predicate != null)
            query = query.Where(predicate);

        return await query
            .FirstOrDefaultAsync();
    }

    public async Task<(int, IEnumerable<T>)> LoadAsync(int page, int size, Expression<Func<T, bool>> predicate = null)
    {
        var query = DbSet.Where(x => x.Status != Status.Deleted);

        if (predicate != null)
            query = query.Where(predicate);

        var count = await query.CountAsync();

        var result = await query
            .Skip((page - 1) * size)
            .Take(size)
            .ToListAsync();

        return (count, result);
    }
    
    public async Task<T> AddAsync(T entity)
    {
        entity.CreatedAt = DateTime.UtcNow;
        DbSet.Add(entity);

        // await _context.SaveChangesAsync();
        return entity;
    }

    public async Task<T> UpdateAsync(T entity)
    {
        entity.UpdatedAt = DateTime.UtcNow;
        _context.Entry(entity).State = EntityState.Modified;

        // await _context.SaveChangesAsync();
        return entity;
    }

    public async Task<bool> DeleteAsync(long id)
    {
        var entity = await GetByIdAsync(id, asTracking: true);
        if (entity != null)
        {
            entity.Status = Status.Deleted;
            entity.UpdatedAt = DateTime.UtcNow;

            // await _context.SaveChangesAsync();
            return true;
        }

        return false;
    }

    public async Task<bool> PermanentDeleteAsync(long id)
    {
        var entity = await GetByIdAsync(id, asTracking: true, withDeleted: true);
        if (entity == null) return false;

        DbSet.Remove(entity);
        // await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> ExistsAsync(long id, bool withDeleted = false)
    {
        var query = DbSet.AsNoTracking();

        if (!withDeleted)
            query = query.Where(x => x.Status != Status.Deleted);

        return await query.AnyAsync(x => x.Id == id);
    }

    public async Task<bool> ExistsAsync(Expression<Func<T, bool>> predicate = null, bool withDeleted = false)
    {
        var query = DbSet.AsNoTracking();

        if (!withDeleted)
            query = query.Where(x => x.Status != Status.Deleted);

        if (predicate != null)
        {
            query = query.Where(predicate);
        }

        return await query.AnyAsync();
    }

    public async Task<int> CountAsync(Expression<Func<T, bool>> predicate = null)
    {
        var query = DbSet.Where(x => x.Status != Status.Deleted);
        if (predicate != null)
            query = query.Where(predicate);

        return await query.CountAsync();
    }

    public async Task<T> RestoreAsync(long id)
    {
        var entity = await DbSet.FirstOrDefaultAsync(x => x.Id == id);
        if (entity == null) return null;

        if (entity.Status != Status.Deleted) return entity;
        
        entity.Status = Status.Active;
        entity.UpdatedAt = DateTime.UtcNow;
        // await _context.SaveChangesAsync();

        return entity;
    }

    public async Task<T> ToggleStatusAsync(long id)
    {
        var entity = await DbSet.FirstOrDefaultAsync(x => x.Id == id);
        if (entity == null) return null;

        entity.Status = entity.Status switch
        {
            Status.Active => Status.Inactive,
            Status.Inactive => Status.Active,
            _ => entity.Status
        };

        entity.UpdatedAt = DateTime.UtcNow;
        // await _context.SaveChangesAsync();

        return entity;
    }

    public IQueryable<T> Query(bool withDeleted = false, bool asTracking = false)
    {
        var query = asTracking ? DbSet.AsQueryable() : DbSet.AsNoTracking();

        if (!withDeleted)
            query = query.Where(x => x.Status != Status.Deleted);

        return query;
    }

    public async Task<(int Count, IEnumerable<T> Items)> ExecuteListAsync(IQueryable<T> queryable, int page = 1, int size = 10)
    {
        var count = await queryable.CountAsync();
        var items = await queryable
            .Skip((page - 1) * size)
            .Take(size)
            .ToListAsync();

        return (count, items);
    }

    public async Task<int> SaveChangesAsync()
    {
        return await _context.SaveChangesAsync();
    }

    public async Task<IDbContextTransaction> BeginTransactionAsync()
    {
        return await _context.Database.BeginTransactionAsync();
    }

    public async Task RefreshEntity(T entity)
    {
        var entry = _context.Entry(entity);
        if (entry == null) return;

        await entry.ReloadAsync();
    }
}