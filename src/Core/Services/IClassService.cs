using Core.App.Services.Interfaces;
using Core.DTOs;
using Core.Entities;
using Core.Errors;

namespace Core.Services;

public interface IClassService : IBaseService<Class>
{
    Task<ApiResponse> SaveAsync(ClassDto classDto);
}