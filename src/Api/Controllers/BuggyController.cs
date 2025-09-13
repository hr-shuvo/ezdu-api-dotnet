using Core.App.Controllers;
using Core.Errors;
using Infrastructure.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

public class BuggyController : BaseApiController
{
    private readonly AppDbContext _context;

    public BuggyController(AppDbContext dbContext)
    {
        _context = dbContext;
    }


    [HttpGet("testauth")]
    [Authorize]
    public ActionResult<string> GetSecretText()
    {
        return "secret stuff";
    }
    
    [HttpGet("notfound")]
    public ActionResult<string> GetNotFoundRequest()
    {
        var thing = _context.Users!.Find(42L);

        if (thing == null)
            throw new AppException(404);
            // return NotFound(new ApiResponse(404));

        return Ok();
    }

    [HttpGet("servererror")]
    public async Task<ActionResult<string>> GetServerError()
    {
        var thing = await _context.Users!.FindAsync(42L);

        var thingRoReturn = thing!.ToString();

        return Ok(thingRoReturn);
    }

    [HttpGet("badrequest")]
    public ActionResult<string> GetBadRequest()
    {
        // return BadRequest(new ApiResponse(400));
        throw new AppException(400);
    }

    [HttpGet("badrequest/{id}")]
    public ActionResult<string> GetBadRequest(long id)
    {
        // return Ok(new ApiResponse(400));
        throw new AppException(400);
    }
}