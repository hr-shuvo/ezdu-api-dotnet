using Core.App.Controllers;
using Core.DTOs;
using Core.Errors;
using Core.QueryParams;
using Core.Services;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

public class TopicContentsController : BaseApiController
{
    private readonly ITopicContentService _topicContentService;

    public TopicContentsController(ITopicContentService topicContentService)
    {
        _topicContentService = topicContentService;
    }
    
    [HttpGet]
    public async Task<IActionResult> GetList([FromQuery] TopicContentParams query)
    {
        var response = await _topicContentService.LoadAsync(query);

        return Ok(response);
    }

    [HttpGet("{id:long}")]
    public async Task<IActionResult> Get(long id)
    {
        var entity = await _topicContentService.GetByIdAsync(id);
        return Ok(entity);
    }


    [HttpPost("save")]
    public async Task<IActionResult> Save([FromBody] TopicContentDto dto)
    {
        var result = await _topicContentService.SaveAsync(dto);

        return Ok(result);
    }

    [HttpDelete("delete/{id:long}")]
    public async Task<IActionResult> Delete(long id)
    {
        if (!await _topicContentService.ExistsAsync(id))
            throw new AppException(404, "Content not found");

        await _topicContentService.SoftDeleteAsync(id);
        await _topicContentService.SaveChangesAsync();
        
        return Ok(new ApiResponse(200, "Content deleted successfully"));
    }

    [HttpDelete("permanent-delete/{id:long}")]
    public async Task<IActionResult> PermanentDelete(long id)
    {
        if (!await _topicContentService.ExistsAsync(x => x.Id == id, true))
            throw new AppException(404, "Content not found");

        await _topicContentService.PermanentDeleteAsync(id);
        await _topicContentService.SaveChangesAsync();
        
        return Ok(new ApiResponse(200, "Content deleted successfully"));
    }
    
    [HttpPatch("restore/{id:long}")]
    public async Task<IActionResult> Restore(long id)
    {
        if (!await _topicContentService.ExistsAsync(x => x.Id == id, true))
            throw new AppException(404, "Content not found");

        var result = await _topicContentService.RestoreAsync(id);
        await _topicContentService.SaveChangesAsync();
        
        return Ok(result);
    }
    
    
    [HttpPatch("toggle-status/{id:long}")]
    public async Task<IActionResult> ToggleStatus(long id)
    {
        if (!await _topicContentService.ExistsAsync(id))
            throw new AppException(404, "Content not found");

        var result =await _topicContentService.ToggleStatusAsync(id);
        await _topicContentService.SaveChangesAsync();
        
        return Ok(result);
    }
}