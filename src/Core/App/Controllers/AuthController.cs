using Core.App.DTOs.Auth;
using Core.App.Services.Interfaces;
using Core.App.Utils;
using Core.Errors;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Core.App.Controllers;

public class AuthController : BaseApiController
{
    private readonly IAuthService _authService;
    // private readonly IUserService _userService;

    public AuthController(IAuthService authService,
        IUserService userService)
    {
        _authService = authService;
        // _userService = userService;
    }


    [Authorize]
    [HttpGet]
    public async Task<IActionResult> GetCurrentUser()
    {
        var user = await _authService.GetCurrentUser(HttpContext.User);

        return Ok(user);
    }


    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginDto loginDto)
    {
        var user = await _authService.LoginAsync(loginDto);
        if (user == null) return Unauthorized();

        var token = user.Token;
        // user.Token = null;

        // Response.Cookies.Append("token", token, new CookieOptions()
        // {
        //     Path = "/",
        //     HttpOnly = true,
        //     Secure = true,
        //     SameSite = SameSiteMode.None,
        //     Expires = DateTime.UtcNow.AddDays(7),
        // });

        return Ok(user);
    }

    [HttpPost("login-mobile")]
    public async Task<IActionResult> LoginMobile(LoginDto loginDto)
    {
        var user = await _authService.LoginAsync(loginDto);
        if (user == null) return Unauthorized();

        if (Request.Cookies.ContainsKey("token"))
        {
            Response.Cookies.Delete("token", new CookieOptions
            {
                Path = "/",
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.None
            });
        }
        
        return Ok(user);
    }

    [HttpGet("logout")]
    public IActionResult Logout()
    {
        if (Request.Cookies.ContainsKey("token"))
        {
            Response.Cookies.Delete("token", new CookieOptions
            {
                Path = "/",
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.None
            });
        }

        return Ok(new ApiResponse(200, "Logout successful"));
    }

    [HttpGet("is-logged-in")]
    public IActionResult IsLoggedIn()
    {
        return Ok(UserContext.UserId > 0);
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterDto registerDto)
    {
        var user = await _authService.RegisterAsync(registerDto);

        return Ok(user);
    }


    [HttpPost("send-otp")]
    public async Task<IActionResult> SendOtp([FromBody] SendOtpDto request)
    {
        var result = await _authService.SendOtpAsync(request);

        var res = new ApiResponse(200, result);

        return Ok(res);
    }

    [HttpPost("verify-otp")]
    public async Task<IActionResult> VerifyOtp([FromBody] VerifyCodeDto request)
    {
        var result = await _authService.VerifyOtpAsync(request);

        var res = new ApiResponse(200, result);

        return Ok(res);
    }
}