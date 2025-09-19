using Core.App.Controllers;
using Core.DTOs;
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
    public async Task<IActionResult> GetClasses()
    {
        var classes = await _classService.LoadAsync();

        return Ok(classes);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetClass(int id)
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
        var result = await _classService.SoftDeleteAsync(id);
        return Ok(result);
    }

    [HttpDelete("permanent-delete/{id:long}")]
    public async Task<IActionResult> PermanentDeleteClass(long id)
    {
        var result = await _classService.PermanentDeleteAsync(id);
        return Ok(result);
    }
}