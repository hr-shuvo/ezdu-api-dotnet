using System.Linq.Expressions;
using Core.App.DTOs;
using Core.App.Entities.Identity;
using Core.App.Repositories.Interfaces;
using Core.App.Services.Interfaces;
using Core.App.Utils;
using Core.Errors;
using Core.QueryParams;
using Core.Shared.Models.Pagination;

namespace Core.App.Services;

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

    public async Task<PagedList<UserDto>> GetUsersAsync(UserParams @params)
    {
        var query = _userRepository.Query(@params.WithDeleted);

        // TODO: Add more filters as needed
        if (!string.IsNullOrWhiteSpace(@params.Search))
        {
            var search = @params.Search.Trim().ToLower();
            query = query.Where(x =>
                x.Firstname.Contains(search, StringComparison.CurrentCultureIgnoreCase) ||
                x.Lastname.Contains(search, StringComparison.CurrentCultureIgnoreCase));
        }
        
        if (@params.OrderBy != null)
        {
            query = @params.OrderBy.ToLower() switch
            {
                "name" => @params.SortBy == "desc"
                    ? query.OrderByDescending(x => x.Firstname)
                    : query.OrderBy(x => x.Firstname),
                "createdat" => @params.SortBy == "desc"
                    ? query.OrderByDescending(x => x.CreatedAt)
                    : query.OrderBy(x => x.CreatedAt),
                "updatedat" => @params.SortBy == "desc"
                    ? query.OrderByDescending(x => x.UpdatedAt)
                    : query.OrderBy(x => x.UpdatedAt),
                _ => query.OrderByDescending(x => x.Id)
            };
        }
        else
        {
            query = @params.SortBy == "desc"
                ? query.OrderByDescending(x => x.CreatedAt)
                : query.OrderBy(x => x.CreatedAt);
        }

        var result = await _userRepository.ExecuteListAsync(query, @params.PageNumber, @params.PageSize);

        var users = result.Items.Select(ToUserDto).ToList();

        return new PagedList<UserDto>(users, result.Count, @params.PageNumber, @params.PageSize);
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

    public async Task<UserDto> UpdateUserAsync(UserDto userDto)
    {
        var currentUserId = UserContext.UserId;

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
            Name = $"{u.Firstname} {u.Lastname}".Trim(),
            CreatedAt = u.CreatedAt,

            Status = u.Status,
        };
    }

    #endregion
}