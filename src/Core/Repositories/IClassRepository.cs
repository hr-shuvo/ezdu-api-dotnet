using Core.App.Repositories.Interfaces;
using Core.Entities;
using Core.QueryParams;

namespace Core.Repositories;

public interface IClassRepository : IBaseRepository<Class>
{
    Task<(int Count, List<Class> Items)> LoadAsync(ClassParams query);
}