using Core.App.Services.Interfaces;
using Core.Entities;

namespace Core.Services;

public interface IProgressService : IBaseService<Progress>
{
    Task<IEnumerable<Progress>> LoadProgress(long userId);
    Task AddXpAsync(int newEntityXp);
}