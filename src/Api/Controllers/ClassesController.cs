using Core.App.Controllers;
using Core.DTOs;
using Core.Entities;
using Core.Enums;
using Core.Errors;
using Core.QueryParams;
using Core.Services;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

public class ClassesController : BaseApiController
{
    private readonly IClassService _classService;

    public ClassesController(IClassService classService)
    {
        _classService = classService;
    }

    [HttpGet]
    public async Task<IActionResult> GetClasses([FromQuery] ClassParams query)
    {
        var classes = await _classService.LoadAsync(query);

        return Ok(classes);
    }

    [HttpGet("{id:long}")]
    public async Task<IActionResult> GetClass(long id)
    {
        var classEntity = await _classService.GetByIdAsync(id);
        return Ok(classEntity);
    }


    [HttpPost("save")]
    public async Task<IActionResult> SaveClass([FromBody] ClassDto classDto)
    {
        var result = await _classService.SaveAsync(classDto);

        return Ok(result);
    }

    [HttpDelete("delete/{id:long}")]
    public async Task<IActionResult> DeleteClass(long id)
    {
        if (!await _classService.ExistsAsync(id))
            throw new AppException(404, "Class not found");

        await _classService.SoftDeleteAsync(id);
        return Ok(new ApiResponse(200, "Class deleted successfully"));
    }

    [HttpDelete("permanent-delete/{id:long}")]
    public async Task<IActionResult> PermanentDeleteClass(long id)
    {
        if (!await _classService.ExistsAsync(x => x.Id == id, true))
            throw new AppException(404, "Class not found");

        await _classService.PermanentDeleteAsync(id);
        return Ok(new ApiResponse(200, "Class deleted successfully"));
    }
}