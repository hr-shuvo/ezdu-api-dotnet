using System.Security.Claims;
using Core.DTOs.Auth;

namespace Core.App.Services;

public interface IAuthService
{
    Task<UserDto> LoginAsync(LoginDto loginDto);
    Task<UserDto> RegisterAsync(RegisterDto registerDto);
    
    Task<bool> ExistsByEmailAsync(string email);
    Task<UserDto> GetCurrentUser(ClaimsPrincipal httpContextUser);
}