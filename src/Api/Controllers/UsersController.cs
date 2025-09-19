using Core.App.Controllers;
using Core.App.DTOs;
using Core.App.Services.Interfaces;
using Core.Extensions;
using Core.QueryParams;
using Infrastructure.Extensions;
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

    [HttpGet("current")]
    public async Task<IActionResult> GetUserProfile()
    {
        var currentUserId = User.GetUserId();
        var user = await _userService.GetByUserIdAsync(currentUserId);
        
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
        var currentUserId = User.GetUserId();
        var updatedUser = await _userService.UpdateUserAsync(currentUserId, user);
        
        return Ok(updatedUser);
    }
}