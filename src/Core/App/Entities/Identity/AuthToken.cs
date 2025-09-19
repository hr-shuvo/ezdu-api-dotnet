using Core.App.DTOs.Common;

namespace Core.App.Entities.Identity;

public class AuthToken : BaseEntity
{
    public long UserId { get; set; }

    public string VerifyToken { get; set; } = string.Empty;
    public string LoginToken { get; set; } = string.Empty;
    
    public DateTime ExpireAt { get; set; }
    public DateTime? UsedAt { get; set; }
    
    // public bool IsUsed => UsedAt.HasValue;
    // public bool IsExpired => DateTime.UtcNow >= ExpireAt;
    
    
}