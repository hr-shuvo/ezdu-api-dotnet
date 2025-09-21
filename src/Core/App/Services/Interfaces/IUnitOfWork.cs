

using Core.App.DTOs.Common;
using Core.App.Repositories.Interfaces;

namespace Core.App.Services.Interfaces;

public interface IUnitOfWork : IDisposable
{
    IBaseRepository<TEntity> Repository<TEntity>() where TEntity : BaseEntity;
    Task<int> CompleteAsync();
}