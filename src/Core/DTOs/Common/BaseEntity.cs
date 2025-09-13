namespace Core.DTOs.Common;

public class BaseEntity
{
    public long Id { get; set; }
    
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; }
    
    public bool IsDeleted { get; set; }
    
    public long? CreatedBy { get; set; }
    public long? UpdatedBy { get; set; }
    
    public void SoftDelete()
    {
        IsDeleted = true;
        UpdatedAt = DateTime.UtcNow;
    }
}