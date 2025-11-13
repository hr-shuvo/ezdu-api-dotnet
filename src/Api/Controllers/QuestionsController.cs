using Core.App.Controllers;
using Core.DTOs;
using Core.Errors;
using Core.QueryParams;
using Core.Services;
using Core.Shared.Models.Pagination;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

public class QuestionsController : BaseApiController
{
    private readonly IQuestionService _questionService;

    public QuestionsController(IQuestionService questionService)
    {
        _questionService = questionService;
    }
    
    [HttpGet]
    public async Task<IActionResult> GetList([FromQuery] QuestionParams query)
    {
        var response = await _questionService.LoadAsync(query);

        return Ok(response);
    }

    [HttpGet("{id:long}")]
    public async Task<IActionResult> Get(long id)
    {
        var entity = await _questionService.GetByIdAsync(id);
        return Ok(entity);
    }


    [HttpPost("save")]
    public async Task<IActionResult> Save([FromBody] QuestionDto dto)
    {
        var result = await _questionService.SaveAsync(dto);

        return Ok(result);
    }

    [HttpDelete("delete/{id:long}")]
    public async Task<IActionResult> Delete(long id)
    {
        if (!await _questionService.ExistsAsync(id))
            throw new AppException(404, "Question not found");

        await _questionService.SoftDeleteAsync(id);
        await _questionService.SaveChangesAsync();
        
        return Ok(new ApiResponse(200, "Question deleted successfully"));
    }

    [HttpDelete("permanent-delete/{id:long}")]
    public async Task<IActionResult> PermanentDelete(long id)
    {
        if (!await _questionService.ExistsAsync(x => x.Id == id, true))
            throw new AppException(404, "Question not found");

        await _questionService.PermanentDeleteAsync(id);
        await _questionService.SaveChangesAsync();
        
        return Ok(new ApiResponse(200, "Question deleted successfully"));
    }
    
    [HttpPatch("restore/{id:long}")]
    public async Task<IActionResult> Restore(long id)
    {
        if (!await _questionService.ExistsAsync(x => x.Id == id, true))
            throw new AppException(404, "Question not found");

        var result = await _questionService.RestoreAsync(id);
        await _questionService.SaveChangesAsync();
        
        return Ok(result);
    }
    
    
    [HttpPatch("toggle-status/{id:long}")]
    public async Task<IActionResult> ToggleStatus(long id)
    {
        if (!await _questionService.ExistsAsync(id))
            throw new AppException(404, "Question not found");

        var result =await _questionService.ToggleStatusAsync(id);
        await _questionService.SaveChangesAsync();
        
        return Ok(result);
    }



    #region Client Request


    [HttpPost("by-topic-ids")]
    public async Task<IActionResult> GetQuestionsByTopicId([FromBody] List<long> ids)
    {
        var result = await _questionService.LoadByTopicIdsAsync(ids);
        
        return Ok(result);
    }

    #endregion
    
    
    
    
    
    


    #region Questions Options

    [HttpPost("{questionId:long}/option-update/{optionId:long}")]
    public async Task<IActionResult> UpdateOption(long questionId, long optionId, [FromBody] OptionDto dto)
    {
        var result = await _questionService.UpdateOptionAsync(questionId, optionId, dto);
        
        return Ok(result);
    }

    [HttpPost("{questionId:long}/option-remove-image/{optionId:long}")]
    public async Task<IActionResult> RemoveOptionImage(long questionId, long optionId)
    {
        await _questionService.RemoveOptionImage(questionId, optionId);
        
        await _questionService.SaveChangesAsync();
        
        return Ok(new ApiResponse(200, "Question image removed successfully"));
    }

    #endregion
}