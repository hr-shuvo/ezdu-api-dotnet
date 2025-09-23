using Core.App.Controllers;
using Core.DTOs;
using Core.Errors;
using Core.QueryParams;
using Core.Services;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

public class TopicsController : BaseApiController
{
    private readonly ITopicService _topicService;

    public TopicsController(ITopicService topicService)
    {
        _topicService = topicService;
    }
    
    [HttpGet]
    public async Task<IActionResult> GetList([FromQuery] TopicParams query)
    {
        var response = await _topicService.LoadAsync(query);

        return Ok(response);
    }

    [HttpGet("{id:long}")]
    public async Task<IActionResult> Get(long id)
    {
        var entity = await _topicService.GetByIdAsync(id);
        return Ok(entity);
    }


    [HttpPost("save")]
    public async Task<IActionResult> Save([FromBody] TopicDto dto)
    {
        var result = await _topicService.SaveAsync(dto);

        return Ok(result);
    }

    [HttpDelete("delete/{id:long}")]
    public async Task<IActionResult> Delete(long id)
    {
        if (!await _topicService.ExistsAsync(id))
            throw new AppException(404, "Topic not found");

        await _topicService.SoftDeleteAsync(id);
        await _topicService.SaveChangesAsync();
        
        return Ok(new ApiResponse(200, "Topic deleted successfully"));
    }

    [HttpDelete("permanent-delete/{id:long}")]
    public async Task<IActionResult> PermanentDelete(long id)
    {
        if (!await _topicService.ExistsAsync(x => x.Id == id, true))
            throw new AppException(404, "Topic not found");

        await _topicService.PermanentDeleteAsync(id);
        await _topicService.SaveChangesAsync();
        
        return Ok(new ApiResponse(200, "Topic deleted successfully"));
    }
    
    [HttpPatch("restore/{id:long}")]
    public async Task<IActionResult> Restore(long id)
    {
        if (!await _topicService.ExistsAsync(x => x.Id == id, true))
            throw new AppException(404, "Topic not found");

        var result = await _topicService.RestoreAsync(id);
        await _topicService.SaveChangesAsync();
        
        return Ok(result);
    }
    
    
    [HttpPatch("toggle-status/{id:long}")]
    public async Task<IActionResult> ToggleStatus(long id)
    {
        if (!await _topicService.ExistsAsync(id))
            throw new AppException(404, "Topic not found");

        var result =await _topicService.ToggleStatusAsync(id);
        await _topicService.SaveChangesAsync();
        
        return Ok(result);
    }
}