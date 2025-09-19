using Core.App.Repositories.Interfaces;
using Core.App.Services;
using Core.App.Services.Identity;
using Core.App.Services.Interfaces;
using Core.Shared.Services;
using Core.Shared.Services.Interfaces;
using Infrastructure.Core;
using Infrastructure.Core.Repositories;
using Infrastructure.Data;
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
        services.AddAppDependencyInjections();

        #region DI

        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped(typeof(IBaseService<>), typeof(BaseService<>));
        services.AddScoped(typeof(IBaseRepository<>), typeof(BaseRepository<>));

        
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<ITokenService, TokenService>();
        services.AddSingleton<IEmailService, EmailService>();
        


        services.AddScoped(typeof(IBaseRepository<>), typeof(BaseRepository<>));
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<ITokenRepository, TokenRepository>();

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