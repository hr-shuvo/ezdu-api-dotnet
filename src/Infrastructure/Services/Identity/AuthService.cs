using System.Security.Claims;
using Core.App.Services;
using Core.DTOs.Auth;
using Core.Entities.Identity;
using Core.Errors;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Services.Identity;

public class AuthService : IAuthService
{
    private readonly UserManager<AppUser> _userManager;
    private readonly ILogger<AuthService> _logger;
    

    public AuthService(UserManager<AppUser> userManager, ILogger<AuthService> logger)
    {
        _userManager = userManager;
        _logger = logger;
    }


    public Task<UserAuthDto> LoginAsync(LoginDto loginDto)
    {
        throw new NotImplementedException();
    }

    public async Task<UserAuthDto> RegisterAsync(RegisterDto registerDto)
    {
        var existingUser = await _userManager.FindByEmailAsync(registerDto.Email);

        if (existingUser is not null)
        {
            throw new AppException("Email is already in use");
        }

        var user = new AppUser
        {
            Email = registerDto.Email,
            UserName = registerDto.Email
        };

        var result = await _userManager.CreateAsync(user, registerDto.Password);

        if (!result.Succeeded)
        {
            throw new AppException(400);
        }

        var userDto = ToUserAuthDto(user);
        
        return userDto;
    }

    public Task<bool> ExistsByEmailAsync(string email)
    {
        throw new NotImplementedException();
    }

    public Task<UserDto> GetCurrentUser(ClaimsPrincipal httpContextUser)
    {
        throw new NotImplementedException();
    }




    #region Private Methods

    private UserAuthDto ToUserAuthDto(AppUser user)
    {
        var userDto = new UserAuthDto
        {
            Username = user.UserName,
            Name = user.UserName
        };
        
        return userDto;
    }

    #endregion
}