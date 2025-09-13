using System.Security.Claims;
using Core.DTOs.Auth;

namespace Core.App.Services;

public interface IAuthService
{
    Task<UserAuthDto> LoginAsync(LoginDto loginDto);
    Task<UserAuthDto> RegisterAsync(RegisterDto registerDto);
    
    Task<bool> ExistsByEmailAsync(string email);
    Task<UserDto> GetCurrentUser(ClaimsPrincipal httpContextUser);
}