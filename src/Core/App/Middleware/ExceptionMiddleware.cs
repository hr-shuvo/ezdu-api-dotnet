
using System.Net;
using System.Text.Json;
using Core.Errors;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Core.App.Middleware;

public class ExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionMiddleware> _logger;
    private readonly IHostEnvironment _env;

    public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger,
        IHostEnvironment env)
    {
        _next = next;
        _logger = logger;
        _env = env;
    }
    
    public async Task InvokeAsync(HttpContext context)
    {
        var options = new JsonSerializerOptions {PropertyNamingPolicy = JsonNamingPolicy.CamelCase};
        
        try
        {
            await _next(context);

            if (context.Response.StatusCode == 401)
            {
                var response = new ApiExceptionResponse(context.Response.StatusCode);
                var json = JsonSerializer.Serialize(response, options);

                await context.Response.WriteAsync(json);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            context.Response.ContentType = "application/json";
            
            int statusCode;
            string message;
            
            if (ex is AppException appEx)
            {
                statusCode = appEx.StatusCode;
                message = appEx.Message;
            }
            else
            {
                statusCode = (int)HttpStatusCode.InternalServerError;
                // message = _env.IsDevelopment() ? ex.Message : "Internal Server Error";
                if (_env.IsDevelopment())
                {
                    var innerMessage = ex.InnerException?.Message;
                    message = innerMessage != null 
                        ? $"{ex.Message} → {innerMessage}"
                        : ex.Message;
                }
                else
                {
                    message = "Internal Server Error";
                }
            }
            
            context.Response.StatusCode = statusCode;

            var response = _env.IsDevelopment()
                ? new ApiExceptionResponse(context.Response.StatusCode, message, ex.StackTrace)
                : new ApiExceptionResponse(context.Response.StatusCode, message);

            var json = JsonSerializer.Serialize(response, options);

            await context.Response.WriteAsync(json);
        }
    }
    
}