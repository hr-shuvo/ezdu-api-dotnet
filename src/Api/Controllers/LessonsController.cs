using Core.App.Controllers;
using Core.DTOs;
using Core.Entities;
using Core.Errors;
using Core.QueryParams;
using Core.Services;
using Core.Shared.Models.Pagination;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[Authorize]
public class LessonsController : BaseApiController
{
    private readonly ILessonService _lessonService;

    public LessonsController(ILessonService lessonService)
    {
        _lessonService = lessonService;
    }

    [HttpGet]
    public async Task<IActionResult> GetList([FromQuery] LessonParams query)
    {
        var response = await _lessonService.LoadAsync(query);

        return Ok(response);
    }

    [HttpGet("{id:long}")]
    public async Task<IActionResult> Get(long id)
    {
        var entity = await _lessonService.GetByIdAsync(id);
        return Ok(entity);
    }


    [HttpPost("save")]
    public async Task<IActionResult> Save([FromBody] LessonDto dto)
    {
        var result = await _lessonService.SaveAsync(dto);

        return Ok(result);
    }

    [HttpDelete("delete/{id:long}")]
    public async Task<IActionResult> Delete(long id)
    {
        if (!await _lessonService.ExistsAsync(id))
            throw new AppException(404, "Lesson not found");

        await _lessonService.SoftDeleteAsync(id);
        await _lessonService.SaveChangesAsync();

        return Ok(new ApiResponse(200, "Lesson deleted successfully"));
    }

    [HttpDelete("permanent-delete/{id:long}")]
    public async Task<IActionResult> PermanentDelete(long id)
    {
        if (!await _lessonService.ExistsAsync(x => x.Id == id, true))
            throw new AppException(404, "Lesson not found");

        await _lessonService.PermanentDeleteAsync(id);
        await _lessonService.SaveChangesAsync();

        return Ok(new ApiResponse(200, "Lesson deleted successfully"));
    }

    [HttpPatch("restore/{id:long}")]
    public async Task<IActionResult> Restore(long id)
    {
        if (!await _lessonService.ExistsAsync(x => x.Id == id, true))
            throw new AppException(404, "Lesson not found");

        var result = await _lessonService.RestoreAsync(id);
        await _lessonService.SaveChangesAsync();

        return Ok(result);
    }


    [HttpPatch("toggle-status/{id:long}")]
    public async Task<IActionResult> ToggleStatus(long id)
    {
        if (!await _lessonService.ExistsAsync(id))
            throw new AppException(404, "Lesson not found");

        var result = await _lessonService.ToggleStatusAsync(id);
        await _lessonService.SaveChangesAsync();

        return Ok(result);
    }

    [HttpGet("withtopics")]
    public async Task<IActionResult> LoadLessonWithTopics([FromQuery] long subjectId = 0)
    {
        if (subjectId is 0)
            throw new AppException(404, "Invalid subject Id");

        var result = await _lessonService.LoadWithTopics(subjectId);

        return Ok(result);
    }
}