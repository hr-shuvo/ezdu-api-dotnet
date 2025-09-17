using Core.Services;
using Core.Services.Interfaces;
using Core.Shared.Services;
using Core.Shared.Services.Interfaces;
using Domain.Interfaces.Repositories;
using Infrastructure.Data;
using Infrastructure.Repositories;
using Infrastructure.Services.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Extensions;

public static class ServiceCollectionExtensions
{
    public static void AddServiceCollections(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDatabaseServices(configuration);
        services.AddAuthenticationServices(configuration);
        services.AddConfigureServices(configuration);

        #region DI

        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<ITokenService, TokenService>();
        services.AddSingleton<IEmailService, EmailService>();









        services.AddScoped<IUserRepository, UserRepository>();

        #endregion

    }

    private static void AddDatabaseServices(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection")!;
        
        services.AddDbContext<AppDbContext>(options =>
        {
            options.UseMySQL(connectionString);
        });
    }
    
    
}