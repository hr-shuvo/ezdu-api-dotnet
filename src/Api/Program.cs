using Core.App.Middleware;
using Core.App.Services.Interfaces;
using Core.App.Utils;
using Infrastructure.Data;
using Infrastructure.Data.Seeds;
using Infrastructure.Extensions;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddSwaggerGen();

builder.Services.AddServiceCollections(builder.Configuration);

var app = builder.Build();


app.UseMiddleware<ExceptionMiddleware>();


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("CorsPolicy");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();


#region Seed

var scope = app.Services.CreateScope();
var services = scope.ServiceProvider;
try
{
    var context = services.GetRequiredService<AppDbContext>();
    await context.Database.MigrateAsync();

    var uow = services.GetRequiredService<IUnitOfWork>();
    var seed = new DefaultAppEntities(uow);
    await seed.SeedAllAsync();
}
catch (Exception ex)
{
    var logger = services.GetRequiredService<ILogger<Program>>();
    logger.LogError(ex, "An error occurred during migration");
}



#endregion


app.Run();