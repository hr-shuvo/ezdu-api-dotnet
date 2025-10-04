using Core.App.DTOs.Common;
using Microsoft.AspNetCore.Identity;

namespace Core.App.Entities.Identity;

public class AppUser : IdentityUser<long>, IBaseEntity
{
    public string Firstname { get; set; }
    public string Lastname { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; }
    
    public int Status { get; set; } = Models.Status.Active;
    
    public long? CreatedBy { get; set; }
    public long? UpdatedBy { get; set; }
}