using Core.App.Controllers;
using Core.DTOs;
using Core.Errors;
using Core.QueryParams;
using Core.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[Authorize]
public class SubjectsController : BaseApiController
{
    private readonly ISubjectService _subjectService;

    public SubjectsController(ISubjectService subjectService)
    {
        _subjectService = subjectService;
    }
    
    [HttpGet]
    public async Task<IActionResult> GetList([FromQuery] SubjectParams query)
    {
        var subjects = await _subjectService.LoadAsync(query);

        return Ok(subjects);
    }

    [HttpGet("{id:long}")]
    public async Task<IActionResult> Get(long id)
    {
        var entity = await _subjectService.GetByIdAsync(id);
        return Ok(entity);
    }


    [HttpPost("save")]
    public async Task<IActionResult> Save([FromBody] SubjectDto dto)
    {
        var result = await _subjectService.SaveAsync(dto);

        return Ok(result);
    }

    [HttpDelete("delete/{id:long}")]
    public async Task<IActionResult> Delete(long id)
    {
        if (!await _subjectService.ExistsAsync(id))
            throw new AppException(404, "Subject not found");

        await _subjectService.SoftDeleteAsync(id);
        await _subjectService.SaveChangesAsync();
        
        return Ok(new ApiResponse(200, "Subject deleted successfully"));
    }

    [HttpDelete("permanent-delete/{id:long}")]
    public async Task<IActionResult> PermanentDelete(long id)
    {
        if (!await _subjectService.ExistsAsync(x => x.Id == id, true))
            throw new AppException(404, "Subject not found");

        await _subjectService.PermanentDeleteAsync(id);
        await _subjectService.SaveChangesAsync();
        
        return Ok(new ApiResponse(200, "Subject deleted successfully"));
    }
    
    [HttpPatch("restore/{id:long}")]
    public async Task<IActionResult> Restore(long id)
    {
        if (!await _subjectService.ExistsAsync(x => x.Id == id, true))
            throw new AppException(404, "Subject not found");

        var result = await _subjectService.RestoreAsync(id);
        await _subjectService.SaveChangesAsync();
        
        return Ok(result);
    }
    
    
    [HttpPatch("toggle-status/{id:long}")]
    public async Task<IActionResult> ToggleStatus(long id)
    {
        if (!await _subjectService.ExistsAsync(id))
            throw new AppException(404, "Subject not found");

        var result =await _subjectService.ToggleStatusAsync(id);
        await _subjectService.SaveChangesAsync();
        
        return Ok(result);
    }
}