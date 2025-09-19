using System.Collections;
using Core.App.DTOs.Common;
using Core.App.Repositories.Interfaces;
using Core.App.Services.Interfaces;
using Infrastructure.Core.Repositories;
using Infrastructure.Data;

namespace Infrastructure.Core;

public class UnitOfWork : IUnitOfWork
{
    private readonly AppDbContext _context;
    private Hashtable _repositories;
    
    public UnitOfWork(AppDbContext context)
    {
        _context = context;
    }
    
    
    public void Dispose()
    {
        _context.Dispose();
    }

    public IBaseRepository<TEntity> Repository<TEntity>() where TEntity : BaseEntity
    {
        _repositories ??= new Hashtable();
        var type = typeof(TEntity).Name;
        
        if (!_repositories.ContainsKey(type))
        {
            var repositoryType = typeof(BaseRepository<>);
            var repositoryInstance = Activator.CreateInstance(repositoryType.MakeGenericType(typeof(TEntity)), _context);
            
            _repositories.Add(type, repositoryInstance);
        }
        
        return (IBaseRepository<TEntity>) _repositories[type];
    }

    public async Task<int> Complete()
    {
        return await _context.SaveChangesAsync();
    }
}