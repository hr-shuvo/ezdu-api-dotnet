using Core.App.DTOs.Auth;
using Core.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
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

        return  Ok(user);
    }

    
    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginDto loginDto)
    {
        var user = await _authService.LoginAsync(loginDto);
        if (user == null) return Unauthorized();
        
        return Ok(user);
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
        await _authService.SendOtpAsync(request);
        return Ok();
    }
    
    
    
}