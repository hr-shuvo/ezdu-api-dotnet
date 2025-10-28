using Core.App.Services.Interfaces;
using Core.DTOs;
using Core.Entities;
using Core.Errors;
using Core.QueryParams;
using Core.Shared.Models.Pagination;

namespace Core.Services;

public interface IExamArchiveService : IBaseService<ExamArchive>
{
    Task<PagedList<ExamArchive>> LoadAsync(ExamArchiveParams query);
    Task<ExamArchive> GetByIdAsync(long id);
    Task<ApiResponse> SaveAsync(ExamArchiveDto classDto);
}