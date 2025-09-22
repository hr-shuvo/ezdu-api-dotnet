namespace Core.App.Models;

public abstract class AppRoles
{
    /// <summary>
    /// Full access to the platform
    /// </summary>
    internal const string SuperAdmin = "SuperAdmin";

    /// <summary>
    /// Full access to the platform
    /// </summary>
    internal const string Admin = "Admin";

    /// <summary>
    /// Full access to moderate content, but no user management
    /// </summary>
    internal const string Moderator = "Moderator";

    /// <summary>
    /// Can contribute content but cannot manage users or moderate content
    /// </summary>
    internal const string Contributor = "Contributor";
    
    /// <summary>
    /// Standard user with access to general features
    /// </summary>
    internal const string Member = "Member";
    
    /// <summary>
    /// Limited access, typically for new or unverified users
    /// </summary>
    internal const string Guest = "Guest";

    /// <summary>
    /// Can access developer tools and APIs
    /// </summary>
    internal const string Developer = "Developer";

    /// <summary>
    /// All defined roles in the system
    /// </summary>
    public static readonly string[] AllRoles =
    [
        SuperAdmin,
        Admin,
        Moderator,
        Member,
        Guest,
        Contributor,
        Developer
    ];
}