using Core.App.Services.Interfaces;
using Core.DTOs;
using Core.Entities;
using Core.Errors;
using Core.QueryParams;
using Core.Shared.Models.Pagination;

namespace Core.Services;

public interface IQuestionService : IBaseService<Question>
{
    Task<PagedList<Question>> LoadAsync(QuestionParams query);
    Task<ApiResponse> SaveAsync(QuestionDto dto);
    Task<ApiResponse> UpdateOptionAsync(long questionId, long optionId, OptionDto dto);
    Task<int> RemoveOptionImage(long questionId, long optionId);
}