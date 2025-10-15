using Core.App.Controllers;
using Core.DTOs;
using Core.Errors;
using Core.QueryParams;
using Core.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[Authorize]
public class QuizzesController : BaseApiController
{
    private readonly IQuizService _quizService;

    public QuizzesController(IQuizService quizService)
    {
        _quizService = quizService;
    }
    
    [HttpGet]
    public async Task<IActionResult> GetList([FromQuery] QuizParams query)
    {
        var classes = await _quizService.LoadAsync(query);

        return Ok(classes);
    }

    [HttpGet("{id:long}")]
    public async Task<IActionResult> Get(long id)
    {
        var classEntity = await _quizService.GetByIdAsync(id);
        return Ok(classEntity);
    }


    [HttpPost("save")]
    public async Task<IActionResult> Save([FromBody] QuizDto classDto)
    {
        var result = await _quizService.SaveAsync(classDto);

        return Ok(result);
    }

    [HttpDelete("delete/{id:long}")]
    public async Task<IActionResult> Delete(long id)
    {
        if (!await _quizService.ExistsAsync(id))
            throw new AppException(404, "Quiz not found");

        await _quizService.SoftDeleteAsync(id);
        await _quizService.SaveChangesAsync();
        
        return Ok(new ApiResponse(200, "Quiz deleted successfully"));
    }

    [HttpDelete("permanent-delete/{id:long}")]
    public async Task<IActionResult> PermanentDelete(long id)
    {
        if (!await _quizService.ExistsAsync(x => x.Id == id, true))
            throw new AppException(404, "Quiz not found");

        await _quizService.PermanentDeleteAsync(id);
        await _quizService.SaveChangesAsync();
        
        return Ok(new ApiResponse(200, "Quiz deleted successfully"));
    }
    
    [HttpPatch("restore/{id:long}")]
    public async Task<IActionResult> Restore(long id)
    {
        if (!await _quizService.ExistsAsync(x => x.Id == id, true))
            throw new AppException(404, "Quiz not found");

        var result = await _quizService.RestoreAsync(id);
        await _quizService.SaveChangesAsync();
        
        return Ok(result);
    }
    
    
    [HttpPatch("toggle-status/{id:long}")]
    public async Task<IActionResult> ToggleStatus(long id)
    {
        if (!await _quizService.ExistsAsync(id))
            throw new AppException(404, "Quiz not found");

        var result =await _quizService.ToggleStatusAsync(id);
        await _quizService.SaveChangesAsync();
        
        return Ok(result);
    }
}