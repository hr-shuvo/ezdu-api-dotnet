using Core.App.Controllers;
using Core.App.DTOs;
using Core.App.Services.Interfaces;
using Core.App.Utils;
using Core.QueryParams;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[Authorize]
public class UsersController : BaseApiController
{
    private readonly IUserService _userService;

    public UsersController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpGet]
    public async Task<IActionResult> GetUsers([FromQuery] UserParams query)
    {
        var users = await _userService.GetUsersAsync(query);
        
        return Ok(users);
    }
    
    [HttpGet("{id:long}")]
    public async Task<IActionResult> Get(long id)
    {
        var user = await _userService.GetByUserIdAsync(id);
        return Ok(user);
    }

    [HttpGet("current")]
    public async Task<IActionResult> GetUserProfile()
    {
        var user = await _userService.GetByUserIdAsync(UserContext.UserId);
        
        return Ok(user);
    }

    [HttpGet("u/{username}")]
    public async Task<IActionResult> GetUser(string username)
    {
        var user = await _userService.GetByUsernameAsync(username);
        
        return Ok(user);
    }


    [HttpPut("update")]
    public async Task<IActionResult> UpdateUser([FromBody] UserDto user)
    {
        var updatedUser = await _userService.UpdateUserAsync(user);
        
        return Ok(updatedUser);
    }
}