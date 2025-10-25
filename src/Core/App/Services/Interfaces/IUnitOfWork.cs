using Core.App.DTOs.Common;
using Core.App.Repositories.Interfaces;
using Core.Repositories;
using Microsoft.EntityFrameworkCore.Storage;

namespace Core.App.Services.Interfaces;

public interface IUnitOfWork : IDisposable
{
    IBaseRepository<TEntity> Repository<TEntity>() where TEntity : BaseEntity;
    Task<int> CompleteAsync();
    Task<IDbContextTransaction> BeginTransactionAsync();

    #region Composit Key Entities
    IQuizQuestionRepository QuizQuestions { get; }
    

    #endregion
}