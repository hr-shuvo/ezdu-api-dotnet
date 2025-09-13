using Core.App.Controllers;
using Core.App.Services;
using Core.DTOs.Auth;
using Core.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

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
    public async Task<ActionResult<UserDto>> GetCurrentUser()
    {
        var user = await _authService.GetCurrentUser(HttpContext.User);

        return  Ok(user);
    }

    
    [HttpPost("login")]
    public async Task<ActionResult<UserAuthDto>> Login(LoginDto loginDto)
    {
        var user = await _authService.LoginAsync(loginDto);
        if (user == null) return Unauthorized();
        
        return user;
    }


    [HttpPost("register")]
    public async Task<ActionResult<UserDto>> Register(RegisterDto registerDto)
    {
        var user = await _authService.RegisterAsync(registerDto);
        
        if (user == null) return BadRequest("Problem registering user");
        
        return Ok(user);
        
    }
}