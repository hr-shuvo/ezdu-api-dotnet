using System.Linq.Expressions;
using Core.App.DTOs.Common;
using Core.App.Models;
using Core.Repositories.Interfaces;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace Infrastructure.Repositories;

public class BaseRepository<T> : IBaseRepository<T> where T : class, IBaseEntity
{
    private readonly AppDbContext _context;
    protected readonly DbSet<T> DbSet;

    public BaseRepository(AppDbContext context)
    {
        _context = context;
        DbSet = context.Set<T>();
    }


    public async Task<T> GetByIdAsync(long id)
    {
        return await DbSet.FirstOrDefaultAsync(x => x.Id == id && x.Status != Status.Deleted);
    }

    public async Task<T> GetAsync(Expression<Func<T, bool>> predicate = null)
    {
        var query = DbSet.Where(x => x.Status != Status.Deleted);

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
        
        var result =  await query
            .Skip((page - 1) * size)
            .Take(size)
            .ToListAsync();
        
        return (count, result);
    }

    public async Task<T> AddAsync(T entity)
    {
        entity.CreatedAt = DateTime.UtcNow;
        DbSet.Add(entity);
        
        await _context.SaveChangesAsync();
        return entity;
    }

    public async Task<T> UpdateAsync(T entity)
    {
        entity.UpdatedAt = DateTime.UtcNow;
        _context.Entry(entity).State = EntityState.Modified;
        
        await _context.SaveChangesAsync();
        return entity;
    }

    public async Task<bool> DeleteAsync(long id)
    {
        var entity = await GetByIdAsync(id);
        if (entity != null)
        {
            entity.Status = Status.Deleted;
            entity.UpdatedAt = DateTime.UtcNow;
            
            await _context.SaveChangesAsync();
            return true;
        }
        return false;
    }

    public async Task<bool> PermanentDeleteAsync(long id)
    {
        var entity = await GetByIdAsync(id);
        if (entity == null) return false;
        
        DbSet.Remove(entity);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> ExistsAsync(long id)
    {
        return await DbSet.AnyAsync(x => x.Id == id && x.Status != Status.Deleted);
    }

    public async Task<bool> ExistsAsync(Expression<Func<T, bool>> predicate = null)
    {
        var query = DbSet.Where(x => x.Status != Status.Deleted);

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

    public IDbContextTransaction BeginTransaction()
    {
        return _context.Database.BeginTransaction();  
    }

    public void RefreshEntity(T entity)
    {
        throw new NotImplementedException();
    }
}