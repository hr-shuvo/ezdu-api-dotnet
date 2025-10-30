using Core.App.Services.Interfaces;
using Core.DTOs;
using Core.Entities;
using Core.Errors;
using Core.QueryParams;
using Core.Shared.Models.Pagination;

namespace Core.Services;

public interface IExamArchiveService : IBaseService<ArchiveExam>
{
    Task<PagedList<ArchiveExam>> LoadAsync(ExamArchiveParams query);
    Task<ArchiveExam> GetByIdAsync(long id);
    Task<ApiResponse> SaveAsync(ExamArchiveDto classDto);
}