using Core.App.Middleware;
using Infrastructure.Extensions;

var builder = WebApplication.CreateBuilder(args);

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