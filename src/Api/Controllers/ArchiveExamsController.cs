using Core.App.Controllers;
using Core.DTOs;
using Core.Errors;
using Core.QueryParams;
using Core.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[Authorize]
public class ArchiveExamsController : BaseApiController
{
    private readonly IExamArchiveService _examArchiveService;

    public ArchiveExamsController(IExamArchiveService examArchiveService)
    {
        _examArchiveService = examArchiveService;
    }
    
    [HttpGet]
    public async Task<IActionResult> GetList([FromQuery] ExamArchiveParams query)
    {
        var classes = await _examArchiveService.LoadAsync(query);

        return Ok(classes);
    }

    [HttpGet("{id:long}")]
    public async Task<IActionResult> Get(long id)
    {
        var classEntity = await _examArchiveService.GetByIdAsync(id);
        return Ok(classEntity);
    }


    [HttpPost("save")]
    public async Task<IActionResult> Save([FromBody] ExamArchiveDto dto)
    {
        var result = await _examArchiveService.SaveAsync(dto);

        return Ok(result);
    }

    [HttpDelete("delete/{id:long}")]
    public async Task<IActionResult> Delete(long id)
    {
        if (!await _examArchiveService.ExistsAsync(id))
            throw new AppException(404, "Exam not found");

        await _examArchiveService.SoftDeleteAsync(id);
        await _examArchiveService.SaveChangesAsync();
        
        return Ok(new ApiResponse(200, "Exam deleted successfully"));
    }

    [HttpDelete("permanent-delete/{id:long}")]
    public async Task<IActionResult> PermanentDelete(long id)
    {
        if (!await _examArchiveService.ExistsAsync(x => x.Id == id, true))
            throw new AppException(404, "Exam not found");

        await _examArchiveService.PermanentDeleteAsync(id);
        await _examArchiveService.SaveChangesAsync();
        
        return Ok(new ApiResponse(200, "Exam deleted successfully"));
    }
    
    [HttpPatch("restore/{id:long}")]
    public async Task<IActionResult> Restore(long id)
    {
        if (!await _examArchiveService.ExistsAsync(x => x.Id == id, true))
            throw new AppException(404, "Exam not found");

        var result = await _examArchiveService.RestoreAsync(id);
        await _examArchiveService.SaveChangesAsync();
        
        return Ok(result);
    }
    
    
    [HttpPatch("toggle-status/{id:long}")]
    public async Task<IActionResult> ToggleStatus(long id)
    {
        if (!await _examArchiveService.ExistsAsync(id))
            throw new AppException(404, "Exam not found");

        var result =await _examArchiveService.ToggleStatusAsync(id);
        await _examArchiveService.SaveChangesAsync();
        
        return Ok(result);
    }


}