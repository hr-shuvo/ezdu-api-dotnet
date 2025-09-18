using Core.App.Controllers;
using Core.QueryParams;
using Core.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

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
}