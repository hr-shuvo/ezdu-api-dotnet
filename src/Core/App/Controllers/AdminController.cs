using Core.App.Models;
using Core.App.Services;
using Core.App.Services.Interfaces;
using Core.Errors;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;

namespace Core.App.Controllers;

public class AdminController : BaseApiController
{
    private readonly ISeeder _seeder;
    private readonly IHostingEnvironment _hostingEnvironment;

    private static readonly Dictionary<string, string[]> PolicyRoles = new()
    {
        [AppPolicies.CanCreate] =
        [
            AppRoles.SuperAdmin,
            AppRoles.Admin,
            AppRoles.Moderator,
            AppRoles.Contributor
        ],
        [AppPolicies.CanUpdate] =
        [
            AppRoles.SuperAdmin,
            AppRoles.Admin,
            AppRoles.Moderator
        ],
        [AppPolicies.CanDelete] =
        [
            AppRoles.SuperAdmin,
            AppRoles.Admin
        ],
        [AppPolicies.CanManageUsers] =
        [
            AppRoles.SuperAdmin,
            AppRoles.Admin
        ],
        [AppPolicies.CanManageRoles] =
        [
            AppRoles.SuperAdmin,
            AppRoles.Admin
        ],
        [AppPolicies.CanViewReports] =
        [
            AppRoles.SuperAdmin,
            AppRoles.Admin,
            AppRoles.Moderator
        ],
        [AppPolicies.CanConfigureSettings] =
        [
            AppRoles.SuperAdmin,
            AppRoles.Admin
        ],
        [AppPolicies.CanAccessAdminPanel] =
        [
            AppRoles.SuperAdmin,
            AppRoles.Admin,
            AppRoles.Moderator
        ]
    };

    public AdminController(ISeeder seeder, IHostingEnvironment hostingEnvironment)
    {
        _seeder = seeder;
        _hostingEnvironment = hostingEnvironment;
    }


    [HttpGet("seed")]
    public async Task<IActionResult> Seed()
    {
        if (!_hostingEnvironment.IsDevelopment())
            throw new AppException(400, "User Seeding is only allowed in Development environment");


        await _seeder.SeedRolesAsync();
        await _seeder.SeedDefaultUsersAsync();

        return Ok("Seeding completed");
    }


    #region Check Access Control

    [Authorize(Policy = AppPolicies.CanCreate)]
    [HttpGet("can-create")]
    public IActionResult CanCreate() => Ok(new { rolesWithAccess = PolicyRoles[AppPolicies.CanCreate] });

    [Authorize(Policy = AppPolicies.CanUpdate)]
    [HttpGet("can-update")]
    public IActionResult CanUpdate() => Ok(new { rolesWithAccess = PolicyRoles[AppPolicies.CanUpdate] });

    [Authorize(Policy = AppPolicies.CanDelete)]
    [HttpGet("can-delete")]
    public IActionResult CanDelete() => Ok(new { rolesWithAccess = PolicyRoles[AppPolicies.CanDelete] });

    [Authorize(Policy = AppPolicies.CanManageUsers)]
    [HttpGet("can-manage-users")]
    public IActionResult CanManageUsers() => Ok(new { rolesWithAccess = PolicyRoles[AppPolicies.CanManageUsers] });

    [Authorize(Policy = AppPolicies.CanManageRoles)]
    [HttpGet("can-manage-roles")]
    public IActionResult CanManageRoles() => Ok(new { rolesWithAccess = PolicyRoles[AppPolicies.CanManageRoles] });

    [Authorize(Policy = AppPolicies.CanViewReports)]
    [HttpGet("can-view-reports")]
    public IActionResult CanViewReports() => Ok(new { rolesWithAccess = PolicyRoles[AppPolicies.CanViewReports] });

    [Authorize(Policy = AppPolicies.CanConfigureSettings)]
    [HttpGet("can-configure-settings")]
    public IActionResult CanConfigureSettings() =>
        Ok(new { rolesWithAccess = PolicyRoles[AppPolicies.CanConfigureSettings] });

    [Authorize(Policy = AppPolicies.CanAccessAdminPanel)]
    [HttpGet("can-access-admin-panel")]
    public IActionResult CanAccessAdminPanel() =>
        Ok(new { rolesWithAccess = PolicyRoles[AppPolicies.CanAccessAdminPanel] });

    #endregion
}