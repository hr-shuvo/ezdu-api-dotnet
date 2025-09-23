using Core.App.Services.Interfaces;
using Core.DTOs;
using Core.Entities;
using Core.Errors;
using Core.QueryParams;
using Core.Shared.Models.Pagination;

namespace Core.Services;

public interface ILessonService : IBaseService<Lesson>
{
    Task<PagedList<Lesson>> LoadAsync(LessonParams query);
    Task<ApiResponse> SaveAsync(LessonDto dto);
}