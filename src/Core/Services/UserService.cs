using System.Linq.Expressions;
using Core.App.DTOs;
using Core.App.Entities.Identity;
using Core.Errors;
using Core.QueryParams;
using Core.Repositories.Interfaces;
using Core.Services.Interfaces;
using Core.Shared.Models.Pagination;

namespace Core.Services;

public class UserService : BaseService<AppUser>, IUserService
{
    private readonly IUserRepository _userRepository;

    public UserService(IUserRepository repository) : base(repository)
    {
        _userRepository = repository;
    }

    public async Task<UserDto> GetByUserIdAsync(long id)
    {
        var result = await _userRepository.GetByIdAsync(id);

        if (result is null)
            throw new AppException(404, "User not found");

        return ToUserDto(result);
    }

    public async Task<PagedList<UserDto>> GetUsersAsync(UserParams query)
    {
        var result = await _userRepository.LoadAsync(query);

        var users = result.Items.Select(ToUserDto).ToList();

        return new PagedList<UserDto>(users, result.Count, query.PageNumber, query.PageSize);
    }

    public async Task<UserDto> GetByUsernameAsync(string query)
    {
        Expression<Func<AppUser, bool>> predicate = u =>
            u.Email == query ||
            u.UserName == query;

        var result = await _userRepository.GetAsync(predicate);

        if (result is null)
            throw new AppException(404, "User not found");

        return ToUserDto(result);
    }

    public async Task<UserDto> UpdateUserAsync(long currentUserId, UserDto userDto)
    {
        if (currentUserId != userDto.Id)
        {
            throw new AppException(400, "You can only update your own profile");
        }

        var user = await _userRepository.GetByIdAsync(currentUserId);

        if (user is null)
            throw new AppException(404, "User not found");

        // user.UserName = userDto.UserName;
        // existingUser.Email = user.Email;
        user.Status = userDto.Status;

        await _userRepository.UpdateAsync(user);

        return ToUserDto(user);
    }


    #region Private Methods

    private UserDto ToUserDto(AppUser u)
    {
        return new UserDto()
        {
            Id = u.Id,
            UserName = u.UserName,
            Email = u.Email,
            Name = u.UserName,
            CreatedAt = u.CreatedAt,

            Status = u.Status,
        };
    }

    #endregion
}