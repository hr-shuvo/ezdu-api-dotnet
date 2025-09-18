using Core.App.DTOs;
using Core.App.Entities.Identity;
using Core.QueryParams;
using Core.Shared.Models.Pagination;

namespace Core.Services.Interfaces;

public interface IUserService : IBaseService<AppUser>
{
    Task<PagedList<UserDto>> GetUsersAsync(UserParams query);
}