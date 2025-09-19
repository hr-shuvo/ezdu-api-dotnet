using System.Security.Claims;
using Core.App.DTOs.Auth;

namespace Core.Services.Interfaces;

public interface IAuthService
{
    Task<UserAuthDto> LoginAsync(LoginDto loginDto);
    Task<UserAuthDto> RegisterAsync(RegisterDto registerDto);
    
    Task<bool> ExistsByEmailAsync(string email);
    Task<UserAuthDto> GetCurrentUser(ClaimsPrincipal httpContextUser);
    
    Task<string> SendOtpAsync(SendOtpDto request);
    Task<string> VerifyOtpAsync(VerifyCodeDto request);
}