using Core.App.Repositories.Interfaces;
using Core.Entities;

namespace Core.Repositories;

public interface IOptionRepository : IBaseRepository<Option>
{
    Task<List<Option>> AddRangeAsync(List<Option> options);
}