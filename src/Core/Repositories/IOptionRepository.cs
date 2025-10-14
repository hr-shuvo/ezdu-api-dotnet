using Core.App.Repositories.Interfaces;
using Core.Entities;

namespace Core.Repositories;

public interface IOptionRepository : IBaseRepository<Option>
{
    Task<List<Option>> LoadOptionsByQuestionIdAsync(long questionId, bool asTracking = false);
    Task<List<Option>> AddRangeAsync(List<Option> options);
    Task<List<Option>> UpdateRangeAsync(List<Option> options);
    Task DeleteRangeAsync(List<Option> optionsToDelete);
}