namespace Core.App.DTOs.Common;

public class BaseEntity : IBaseEntity
{
    public long Id { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; }

    public int Status { get; set; } = Models.Status.Active;

    public long? CreatedBy { get; set; }
    public long? UpdatedBy { get; set; }

    public void SoftDelete()
    {
        Status = Models.Status.Deleted;
        UpdatedAt = DateTime.UtcNow;
    }
}