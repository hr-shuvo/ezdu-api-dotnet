using System.Security.Claims;
using Core.App.Services;
using Core.DTOs.Auth;
using Core.Entities.Identity;
using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Services.Identity;

public class AuthService : IAuthService
{
    private readonly UserManager<AppUser> _userManager;
    

    public AuthService(UserManager<AppUser> userManager)
    {
        _userManager = userManager;
    }


    public Task<UserDto> LoginAsync(LoginDto loginDto)
    {
        throw new NotImplementedException();
    }

    public async Task<UserDto> RegisterAsync(RegisterDto registerDto)
    {
        try
        {
            var existingUser = await _userManager.FindByEmailAsync(registerDto.Email);

            if (existingUser is not null)
            {
                throw new Exception("Email is already in use");
            }
            
            throw new NotImplementedException();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public Task<bool> ExistsByEmailAsync(string email)
    {
        throw new NotImplementedException();
    }

    public Task<UserDto> GetCurrentUser(ClaimsPrincipal httpContextUser)
    {
        throw new NotImplementedException();
    }
}