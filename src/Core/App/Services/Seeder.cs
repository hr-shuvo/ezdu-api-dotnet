using System.Security.Claims;
using Core.App.Entities.Identity;
using Core.App.Models;
using Core.App.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Core.App.Services;

public class Seeder : ISeeder
{
    private readonly RoleManager<AppRole> _roleManager;
    private readonly UserManager<AppUser> _userManager;

    public Seeder(RoleManager<AppRole> roleManager, UserManager<AppUser> userManager)
    {
        _roleManager = roleManager;
        _userManager = userManager;
    }

    public async Task SeedRolesAsync()
    {
        if (await _roleManager.Roles.AnyAsync()) return;

        foreach (var role in AppRoles.AllRoles)
        {
            if (!await _roleManager.RoleExistsAsync(role))
            {
                await _roleManager.CreateAsync(new AppRole { Name = role });
            }
        }
    }

    public async Task SeedDefaultUsersAsync()
    {
        if (await _userManager.Users.AnyAsync()) return;

        foreach (var role in AppRoles.AllRoles)
        {
            var defaultUserName = role.ToLower() + "@example.com";
            if (await _userManager.FindByNameAsync(defaultUserName) == null)
            {
                var user = new AppUser
                {
                    Firstname = role,
                    UserName = defaultUserName,
                    Email = defaultUserName,
                    EmailConfirmed = true
                };

                var result = await _userManager.CreateAsync(user, "123456");
                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(user, role);
                    await AddDefaultClaimsToUserAsync(user, role);
                }
            }
        }
    }
    

    private async Task AddDefaultClaimsToUserAsync(AppUser user, string role)
    {
        var claims = GetDefaultClaimsForRole(role);

        foreach (var claim in claims)
        {
            await _userManager.AddClaimAsync(user, claim);
        }
    }

    private static List<Claim> GetDefaultClaimsForRole(string role)
    {
        return role switch
        {
            AppRoles.SuperAdmin =>
            [
                new Claim(AppActions.Create, "true"),
                new Claim(AppActions.Read, "true"),
                new Claim(AppActions.Update, "true"),
                new Claim(AppActions.Delete, "true"),
                new Claim(AppActions.ManageUsers, "true"),
                new Claim(AppActions.ManageRoles, "true"),
                new Claim(AppActions.ViewReports, "true"),
                new Claim(AppActions.ConfigureSettings, "true"),
                new Claim(AppActions.AccessAdminPanel, "true"),
                new Claim(AppActions.Submit, "true"),
                new Claim(AppActions.Approve, "true"),
                new Claim(AppActions.Reject, "true")
            ],
            AppRoles.Admin =>
            [
                new Claim(AppActions.Create, "true"),
                new Claim(AppActions.Read, "true"),
                new Claim(AppActions.Update, "true"),
                new Claim(AppActions.Delete, "true"),
                new Claim(AppActions.ManageUsers, "true"),
                new Claim(AppActions.ManageRoles, "true"),
                new Claim(AppActions.ViewReports, "true"),
                new Claim(AppActions.AccessAdminPanel, "true"),
                new Claim(AppActions.Submit, "true"),
                new Claim(AppActions.Approve, "true"),
                new Claim(AppActions.Reject, "true")
            ],
            AppRoles.Moderator =>
            [
                new Claim(AppActions.Create, "true"),
                new Claim(AppActions.Read, "true"),
                new Claim(AppActions.Update, "true"),
                new Claim(AppActions.Delete, "true"),
                new Claim(AppActions.ViewReports, "true"),
                new Claim(AppActions.Submit, "true"),
                new Claim(AppActions.Approve, "true"),
                new Claim(AppActions.Reject, "true")
            ],
            AppRoles.Contributor =>
            [
                new Claim(AppActions.Create, "true"),
                new Claim(AppActions.Read, "true"),
                new Claim(AppActions.Update, "true"),
                new Claim(AppActions.Delete, "true"),
                new Claim(AppActions.Submit, "true")
            ],
            AppRoles.Member => [],
            AppRoles.Guest => [],
            AppRoles.Developer => [],
            _ => []
        };
    }
}