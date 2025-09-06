using Domain.Entities.Common;
using Microsoft.AspNetCore.Identity;

namespace Domain.Entities.Identity;

public class AppUser : IdentityUser<long>, IBaseEntity
{
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public bool IsDeleted { get; set; }
    public long? CreatedBy { get; set; }
    public long? UpdatedBy { get; set; }
}