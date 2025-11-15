namespace Core.DTOs;

public class LeaderboardDto
{
    public long Rank { get; set; }
    public long UserId { get; set; }
    public string UserName { get; set; }
    public string UserImageUrl { get; set; }
    public int TotalXp { get; set; }
    public int StreakCount { get; set; }
}