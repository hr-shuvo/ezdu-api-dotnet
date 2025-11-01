using Core.App.Services.Interfaces;
using Core.DTOs;
using Core.Entities;
using Core.Errors;
using Core.QueryParams;
using Core.Shared.Models.Pagination;

namespace Core.Services;

public interface IUserQuizService : IBaseService<UserQuiz>
{
    Task<PagedList<UserQuiz>> LoadAsync(UserQuizParams query);
    Task<ApiResponse> SaveAsync(UserQuizDto dto);
}