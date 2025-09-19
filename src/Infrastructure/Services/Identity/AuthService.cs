using System.Security.Claims;
using Core.App.DTOs.Auth;
using Core.App.Entities.Identity;
using Core.App.Utils;
using Core.Errors;
using Core.Services.Interfaces;
using Core.Shared.Models.Messaging;
using Core.Shared.Services.Interfaces;
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

    private readonly IEmailService _emailService;


    public AuthService(UserManager<AppUser> userManager,
        SignInManager<AppUser> signInManager,
        ITokenService tokenService,
        ILogger<AuthService> logger,
        IEmailService emailService)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _tokenService = tokenService;
        _logger = logger;
        _emailService = emailService;
    }


    #region Auth Methods

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

                #region Send Login Notification Email

                var message = new EmailMessage()
                {
                    To = user.Email,
                    ToName = user.UserName,
                    Subject = "New Login Notification",
                    Body =
                        $"Hello {user.UserName},\n\nWe noticed a new login to your account " +
                        $"on {DateTime.UtcNow} UTC.\n\nIf this was you, no further action is needed.\n\n" +
                        $"If you did not log in, please reset your password immediately and contact support.\n\n" +
                        $"Best regards,\nYour Company",
                    IsHtml = true,
                };

                bool emailSent = await _emailService.SendEmailAsync(message);
                if (!emailSent)
                {
                    _logger.LogWarning("Failed to send login notification email to {Email}", user.Email);
                }

                #endregion


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

    public async Task<UserAuthDto> GetCurrentUser(ClaimsPrincipal contextUser)
    {
        try
        {
            var username = contextUser.GetUsername();

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

    #endregion


    #region OPT Methods

    public async Task<string> SendOtpAsync(SendOtpDto request)
    {
        try
        {
            AppUser user;

            if (request.IsPhone)
            {
                var recipient = NormalizeHelper.NormalizePhoneNumber(request.Recipient);

                user = await _userManager.Users.FirstOrDefaultAsync(u => u.PhoneNumber == recipient);
            }
            else
            {
                user = await _userManager.Users.FirstOrDefaultAsync(u => u.Email == request.Recipient);
            }

            if (user == null)
            {
                throw new AppException(404, "No account associated with the provided recipient");
            }

            var userValidToken = await _tokenService.GetAsync(x =>
                x.ExpireAt > DateTime.UtcNow &&
                x.UserId == user.Id, withDeleted: true);

            if (userValidToken is null)
            {
                // await _tokenService.DeleteAllByUserIdAsync(user.Id);
                var userToken = await _tokenService.GetAsync(x =>
                    x.UserId == user.Id, withDeleted: true);

                var loginCode = Helper.GenerateLoginCode(4);
                var encryptedCode = _tokenService.EncryptCode(loginCode);

                if (userToken is null)
                {
                    userValidToken = new AuthToken
                    {
                        UserId = user.Id,
                        LoginToken = encryptedCode,
                        CreatedAt = DateTime.UtcNow,
                        ExpireAt = DateTime.UtcNow.AddMinutes(5)
                    };

                    userValidToken = await _tokenService.AddAsync(userValidToken);
                }
                else
                {
                    userToken.ExpireAt = DateTime.UtcNow.AddMinutes(5);
                    userToken.CreatedAt = DateTime.UtcNow;
                    userToken.LoginToken = encryptedCode;

                    userValidToken = await _tokenService.UpdateAsync(userToken);
                }
            }

            var code = userValidToken.LoginToken;
            var decryptedCode = _tokenService.DecryptCode(code);


            if (request.IsPhone)
            {
                // Send SMS

                var smsMessage = new SmsMessage
                {
                    To = user.PhoneNumber,
                    Body = $"Your login code is: {decryptedCode}"
                };


                return $"Code sent to {request.Recipient} successfully";

                throw new AppException(400, "Failed to send email. Please try again later.");
            }
            else
            {
                // Send Email

                var emailMessage = new EmailMessage
                {
                    To = user.Email,
                    ToName = user.UserName,
                    Subject = "Your Login Code",
                    Body = $"Your login code is: {decryptedCode}",
                    IsHtml = false
                };

                var emailSent = await _emailService.SendEmailAsync(emailMessage);

                if (emailSent)
                    return $"Code sent to {request.Recipient} successfully";

                throw new AppException(400, "Failed to send email. Please try again later.");
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error sending login code to {Recipient}", request.Recipient);
            throw;
        }
    }

    public async Task<string> VerifyOtpAsync(VerifyCodeDto request)
    {
        try
        {
            AppUser user;

            if (Helper.IsValidEmail(request.Recipient))
            {
                user = await _userManager.Users.FirstOrDefaultAsync(u => u.Email == request.Recipient);
            }
            else
            {
                var recipient = NormalizeHelper.NormalizePhoneNumber(request.Recipient);
                user = await _userManager.Users.FirstOrDefaultAsync(u => u.PhoneNumber == recipient);
            }

            if (user == null)
            {
                throw new AppException(404, "No account associated with the provided recipient");
            }

            if (user.EmailConfirmed && Helper.IsValidEmail(request.Recipient) && user.Email == request.Recipient)
                throw new AppException(400, "Email already verified");

            if (user.PhoneNumberConfirmed && !Helper.IsValidEmail(request.Recipient) &&
                user.PhoneNumber == request.Recipient)
                throw new AppException(400, "Phone number already verified");

            var userToken = await _tokenService.GetAsync(x =>
                x.ExpireAt > DateTime.UtcNow);

            if (userToken is null)
            {
                throw new AppException(400, "Invalid or expired code");
            }

            var decryptedCode = _tokenService.DecryptCode(userToken.LoginToken);
            if (decryptedCode != request.Code)
            {
                throw new AppException(400, "Invalid code");
            }

            if (Helper.IsValidEmail(request.Recipient) && user.Email == request.Recipient)
            {
                user.EmailConfirmed = true;
            }
            else if (!Helper.IsValidEmail(request.Recipient) && user.PhoneNumber == request.Recipient)
            {
                user.PhoneNumberConfirmed = true;
            }
            else
            {
                throw new AppException(400, "Recipient does not match user");
            }

            await _userManager.UpdateAsync(user);

            return "Code verified successfully";
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error verifying login code: {Code}", request.Code);
            throw;
        }
    }

    #endregion


    #region Private Methods

    private UserAuthDto ToUserAuthDto(AppUser user)
    {
        var userDto = new UserAuthDto
        {
            Username = user.UserName,
            Name = user.UserName, // todo: add full name property
            Token = _tokenService.CreateAuthToken(user)
        };

        return userDto;
    }

    #endregion
}