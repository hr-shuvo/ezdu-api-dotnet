using Core.Repositories;
using Core.Services;
using Infrastructure.Repositories;
using Infrastructure.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Extensions;

public static class AppDependencyExtensions
{
    public static void AddAppDependencyInjections(this IServiceCollection services)
    {

        services.AddScoped<IClassService, ClassService>();
        
        
        
        
        
        
        
        
        services.AddScoped<IClassRepository, ClassRepository>();
    }
}