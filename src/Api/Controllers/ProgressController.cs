using Core.App.Controllers;
using Core.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[Authorize]
public class ProgressController : BaseApiController
{
    private readonly IProgressService _progressService;

    public ProgressController(IProgressService progressService)
    {
        _progressService = progressService;
    }

    [HttpGet]
    public async Task<IActionResult> GetProgress()
    {
        var result = await _progressService.GetAsync();

        return Ok(result);
    }

    [HttpGet("{userId:long}")]
    public async Task<IActionResult> GetProgress(long userId)
    {
        var progress = await _progressService.LoadProgress(userId);

        return Ok(progress);
    }
    
    
    
}