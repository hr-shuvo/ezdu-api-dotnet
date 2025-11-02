using Core.App.Controllers;
using Core.DTOs;
using Core.Errors;
using Core.QueryParams;
using Core.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[Authorize]
public class UserQuizController : BaseApiController
{
    private readonly IUserQuizService _userQuizService;

    public UserQuizController(IUserQuizService userQuizService)
    {
        _userQuizService = userQuizService;
    }

    public async Task<IActionResult> GetList([FromQuery] UserQuizParams query)
    {
        var result = await _userQuizService.LoadAsync(query);
        
        return Ok(result);
    }
    
    [HttpPost("save")]
    public async Task<IActionResult> Save([FromBody] UserQuizDto dto)
    {
        var result = await _userQuizService.SaveAsync(dto);

        // return Ok(result);
        return Ok(new ApiResponse(200, "Congratulations!"));
    }
}