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


// if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("CorsPolicy");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.MapGet("/", () => "Ezdu - API is running...");

app.Run();