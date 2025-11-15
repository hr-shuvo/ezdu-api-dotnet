using Core.App.DTOs.Common;
using Core.ValueObject;

namespace Core.Entities;

public class Progress: BaseEntity
{
    public long UserId { get; set; }

    public string  UserName { get; set; }
    public string UserImageUrl { get; set; }

    public int TotalXp { get; set; }
    public int WeekXp { get; set; }
    public DateTime LastStreakDay { get; set; }
    public DateTime LastResetDate { get; set; }
    public int StreakCount { get; set; }

    public ICollection<DailyXp> DailyXps { get; set; } = [];

    public Progress(long userId)
    {
        Id = userId;
        UserId = userId;
    }
}