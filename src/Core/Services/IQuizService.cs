using Core.App.Services.Interfaces;
using Core.DTOs;
using Core.Entities;
using Core.Errors;
using Core.QueryParams;
using Core.Shared.Models.Pagination;

namespace Core.Services;

public interface IQuizService : IBaseService<Quiz>
{
    Task<PagedList<Quiz>> LoadAsync(QuizParams query);
    new Task<Quiz> GetByIdAsync(long id, bool asTracking = false, bool withDeleted = false);
    Task<QuizResponseDto> GetByIdWithQuestionsAsync(long id, bool asTracking = false, bool withDeleted = false);
    Task<ApiResponse> SaveAsync(QuizDto classDto);
}