namespace Core.App.Models;

public static class AppRoleGroups
{
    public static readonly string[] SuperAdmin = 
    [
        AppRoles.SuperAdmin
    ];
    
    public static readonly string[] Admin = 
    [
        AppRoles.SuperAdmin,
        AppRoles.Admin
    ];
    
    public static readonly string[] Moderator = 
    [
        AppRoles.SuperAdmin,
        AppRoles.Admin,
        AppRoles.Moderator
    ];
    
    
    public static readonly string[] Contributor = 
    [
        AppRoles.SuperAdmin,
        AppRoles.Admin,
        AppRoles.Moderator,
        AppRoles.Contributor
    ];
    
    public static readonly string[] Member = 
    [
        AppRoles.SuperAdmin,
        AppRoles.Admin,
        AppRoles.Moderator,
        AppRoles.Contributor,
        AppRoles.Member
    ];
    
    public static readonly string[] Guest = 
    [
        AppRoles.SuperAdmin,
        AppRoles.Admin,
        AppRoles.Moderator,
        AppRoles.Contributor,
        AppRoles.Member,
        AppRoles.Guest
    ];
    
    
    public static readonly string[] Developer = 
    [
        AppRoles.SuperAdmin,
        AppRoles.Admin,
        AppRoles.Developer
    ];
}