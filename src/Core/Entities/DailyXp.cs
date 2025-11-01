using Core.App.DTOs.Common;

namespace Core.Entities;

public class DailyXp : BaseEntity
{
    public DateTime Day { get; set; }
    public int Xp { get; set; }

    public long UserId { get; set; }
    // public long ProgressId { get; set; }
}