using Core.App.DTOs;
using Core.App.Entities.Identity;
using Core.QueryParams;
using Core.Shared.Models.Pagination;

namespace Core.App.Services.Interfaces;

public interface IUserService : IBaseService<AppUser>
{
    public Task<UserDto> GetByUserIdAsync(long id);
    Task<PagedList<UserDto>> GetUsersAsync(UserParams query);
    Task<UserDto> GetByUsernameAsync(string query);
    Task<UserDto> UpdateUserAsync(long currentUserId, UserDto user);
}