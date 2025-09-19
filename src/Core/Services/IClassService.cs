using Core.App.Services.Interfaces;
using Core.DTOs;
using Core.Entities;
using Core.Errors;
using Core.QueryParams;
using Core.Shared.Models.Pagination;

namespace Core.Services;

public interface IClassService : IBaseService<Class>
{
    Task<PagedList<Class>> LoadAsync(ClassParams query);
    Task<ApiResponse> SaveAsync(ClassDto classDto);
}