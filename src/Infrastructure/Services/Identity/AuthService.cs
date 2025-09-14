using System.Security.Claims;
using Core.DTOs.Auth;
using Core.Entities.Identity;
using Core.Errors;
using Core.Services.Interfaces;
using Infrastructure.Extensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Services.Identity;

public class AuthService : IAuthService
{
    private readonly UserManager<AppUser> _userManager;
    private readonly SignInManager<AppUser> _signInManager;
    private readonly ITokenService _tokenService;
    private readonly ILogger<AuthService> _logger;


    public AuthService(UserManager<AppUser> userManager,
        SignInManager<AppUser> signInManager,
        ITokenService tokenService,
        ILogger<AuthService> logger)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _tokenService = tokenService;
        _logger = logger;
    }


    public async Task<UserAuthDto> LoginAsync(LoginDto loginDto)
    {
        try
        {
            var user = await _userManager.Users.FirstOrDefaultAsync(u =>
                u.UserName == loginDto.Username ||
                u.Email == loginDto.Username ||
                u.PhoneNumber == loginDto.Username);

            if (user == null)
            {
                throw new AppException(401, "Invalid username or password");
            }

            var result = await _signInManager.CheckPasswordSignInAsync(user, loginDto.Password, true);

            if (result.Succeeded)
            {
                var userDto = ToUserAuthDto(user);


                return userDto;
            }

            if (result.IsLockedOut)
            {
                throw new AppException(403,
                    "User account locked due to multiple failed login attempts. Please try again later or contact support.");
            }

            if (result.IsNotAllowed)
            {
                throw new AppException(403, "User account is not allowed");
            }

            if (result.RequiresTwoFactor)
            {
                throw new AppException(403, "User account requires 2 factor authentication");
            }

            var accessFailedCount = await _userManager.GetAccessFailedCountAsync(user);
            var maxAttempts = _userManager.Options.Lockout.MaxFailedAccessAttempts;
            var attemptsLeft = maxAttempts - accessFailedCount;

            if (attemptsLeft > 0)
            {
                throw new AppException(401,
                    $"Invalid username or password. You have {attemptsLeft} attempts remaining.");
            }

            throw new AppException(401,
                "Invalid username or password. Warning: Your account will be locked after the next failed attempt.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error during user login for Username: {Username}",
                loginDto.Username);

            throw;
        }
    }

    public async Task<UserAuthDto> RegisterAsync(RegisterDto registerDto)
    {
        try
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
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error during user registration for email: {Email}",
                registerDto.Email);

            throw;
        }
    }

    public async Task<bool> ExistsByEmailAsync(string email)
    {
        try
        {
            return await _userManager.FindByEmailAsync(email) != null;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error checking existence of user by email: {Email}", email);
            throw;
        }
    }

    public async Task<UserAuthDto> GetCurrentUser(ClaimsPrincipal User)
    {
        try
        {
            var username = User.GetUsername();
            var userId = User.GetUserId();

            if (string.IsNullOrEmpty(username))
            {
                throw new AppException(401, "User is not authenticated");
            }

            var user = await _userManager.FindByNameAsync(username);

            if (user == null)
            {
                throw new AppException(404, "User not found");
            }

            var userDto = ToUserAuthDto(user);

            return userDto;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving current user from claims principal");
            throw;
        }
    }


    #region Private Methods

    private UserAuthDto ToUserAuthDto(AppUser user)
    {
        var userDto = new UserAuthDto
        {
            Username = user.UserName,
            Name = user.UserName, // todo: add full name property
            Token = _tokenService.CreateToken(user)
        };

        return userDto;
    }

    #endregion
}