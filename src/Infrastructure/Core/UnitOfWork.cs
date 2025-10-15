using System.Collections;
using Core.App.DTOs.Common;
using Core.App.Repositories.Interfaces;
using Core.App.Services.Interfaces;
using Core.Repositories;
using Infrastructure.Core.Repositories;
using Infrastructure.Data;
using Infrastructure.Repositories;

namespace Infrastructure.Core;

public class UnitOfWork : IUnitOfWork
{
    private readonly AppDbContext _context;
    private Hashtable _repositories;

    
    public UnitOfWork(AppDbContext context)
    {
        _context = context;

        QuizQuestions = new QuizQuestionRepository(context);
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
            var repositoryInstance =
                Activator.CreateInstance(repositoryType.MakeGenericType(typeof(TEntity)), _context);

            _repositories.Add(type, repositoryInstance);
        }

        return (IBaseRepository<TEntity>)_repositories[type];
    }

    public async Task<int> CompleteAsync()
    {
        return await _context.SaveChangesAsync();
    }
    
    

    #region Composit Key Entities

    public IQuizQuestionRepository QuizQuestions { get; }

    #endregion
}