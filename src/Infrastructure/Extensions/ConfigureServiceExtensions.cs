using Core.App.Utils;
using Core.Errors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Extensions;

public static class ConfigureServiceExtensions
{
    public static void AddConfigureServices(this IServiceCollection services, IConfiguration config)
    {
        // services.AddCors(opt =>
        // {
        //     opt.AddPolicy("CorsPolicy", policy =>
        //     {
        //         policy.AllowAnyHeader().AllowAnyMethod().WithOrigins("");
        //     });
        // });
        
        services.Configure<ApiBehaviorOptions>(options =>
        {
            options.InvalidModelStateResponseFactory = actionContext =>
            {
                var errors = actionContext.ModelState
                    .Where(e => e.Value.Errors.Count > 0)
                    .SelectMany(x => x.Value.Errors)
                    .Select(x => x.ErrorMessage).ToArray();

                var errorResponse = new ApiValidationErrorResponse
                {
                    Errors = errors
                };

                return new BadRequestObjectResult(errorResponse);
            };
        });

        services.AddSingleton<IHttpContextAccessor>(x =>
        {
            var accessor = new HttpContextAccessor();
            UserContext.Configure(accessor);
            
            return accessor;
        });

    }

}