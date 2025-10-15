using Core.App.DTOs.Common;
using Core.App.Repositories.Interfaces;
using Core.Repositories;

namespace Core.App.Services.Interfaces;

public interface IUnitOfWork : IDisposable
{
    IBaseRepository<TEntity> Repository<TEntity>() where TEntity : BaseEntity;
    Task<int> CompleteAsync();

    #region Composit Key Entities
    IQuizQuestionRepository QuizQuestions { get; }
    

    #endregion
}