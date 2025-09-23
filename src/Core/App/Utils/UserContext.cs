using System.Security.Claims;
using Microsoft.AspNetCore.Http;

namespace Core.App.Utils;

public static class UserContext
{
    private static IHttpContextAccessor _httpContextAccessor;

    public static void Configure(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    private static ClaimsPrincipal User => _httpContextAccessor?.HttpContext?.User;

    public static long UserId =>
        long.Parse(User?.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");

    public static string Username => User?.FindFirst(ClaimTypes.Name)?.Value;

    public static string Email => User?.FindFirst(ClaimTypes.Email)?.Value;

    public static bool IsAuthenticated => User?.Identity?.IsAuthenticated ?? false;

    public static bool IsInRole(string role) => User?.IsInRole(role) ?? false;
}