using Core.App.Controllers;
using Core.DTOs;
using Core.Errors;
using Core.QueryParams;
using Core.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[Authorize]
public class ClassesController : BaseApiController
{
    private readonly IClassService _classService;

    public ClassesController(IClassService classService)
    {
        _classService = classService;
    }

    [HttpGet]
    public async Task<IActionResult> GetList([FromQuery] ClassParams query)
    {
        var classes = await _classService.LoadAsync(query);

        return Ok(classes);
    }

    [HttpGet("{id:long}")]
    public async Task<IActionResult> Get(long id)
    {
        var classEntity = await _classService.GetByIdAsync(id);
        return Ok(classEntity);
    }


    [HttpPost("save")]
    public async Task<IActionResult> Save([FromBody] ClassDto classDto)
    {
        var result = await _classService.SaveAsync(classDto);

        return Ok(result);
    }

    [HttpDelete("delete/{id:long}")]
    public async Task<IActionResult> Delete(long id)
    {
        if (!await _classService.ExistsAsync(id))
            throw new AppException(404, "Class not found");

        await _classService.SoftDeleteAsync(id);
        await _classService.SaveChangesAsync();
        
        return Ok(new ApiResponse(200, "Class deleted successfully"));
    }

    [HttpDelete("permanent-delete/{id:long}")]
    public async Task<IActionResult> PermanentDelete(long id)
    {
        if (!await _classService.ExistsAsync(x => x.Id == id, true))
            throw new AppException(404, "Class not found");

        await _classService.PermanentDeleteAsync(id);
        await _classService.SaveChangesAsync();
        
        return Ok(new ApiResponse(200, "Class deleted successfully"));
    }
    
    [HttpPatch("restore/{id:long}")]
    public async Task<IActionResult> Restore(long id)
    {
        if (!await _classService.ExistsAsync(x => x.Id == id, true))
            throw new AppException(404, "Class not found");

        var result = await _classService.RestoreAsync(id);
        await _classService.SaveChangesAsync();
        
        return Ok(result);
    }
    
    
    [HttpPatch("toggle-status/{id:long}")]
    public async Task<IActionResult> ToggleStatus(long id)
    {
        if (!await _classService.ExistsAsync(id))
            throw new AppException(404, "Class not found");

        var result =await _classService.ToggleStatusAsync(id);
        await _classService.SaveChangesAsync();
        
        return Ok(result);
    }
}