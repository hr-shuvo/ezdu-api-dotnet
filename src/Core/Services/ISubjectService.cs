using Core.App.Services.Interfaces;
using Core.DTOs;
using Core.Entities;
using Core.Errors;
using Core.QueryParams;
using Core.Shared.Models.Pagination;

namespace Core.Services;

public interface ISubjectService : IBaseService<Subject>
{
    Task<PagedList<Subject>> LoadAsync(SubjectParams query);
    Task<ApiResponse> SaveAsync(SubjectDto classDto);
}