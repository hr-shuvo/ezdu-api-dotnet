using Core.App.Controllers;
using Core.App.Services.Interfaces;
using Infrastructure.Data;
using Infrastructure.Data.Seeds;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Api.Controllers;

public class ZSeedController : BaseApiController
{
    private readonly AppDbContext _context;
    private readonly IUnitOfWork _unitOfWork;

    public ZSeedController(AppDbContext context, IUnitOfWork unitOfWork)
    {
        _context = context;
        _unitOfWork = unitOfWork;
    }

    [HttpGet]
    public async Task<IActionResult> SeedDefaultEntitiesAsync()
    {
        await _context.Database.MigrateAsync();
        
        var seed = new DefaultAppEntities(_unitOfWork);
        await seed.SeedAllAsync();
        
        return Ok("Seeding default entities");
    }

}