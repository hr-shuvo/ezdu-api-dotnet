using Core.App.DTOs.Common;
using Core.Enums;

namespace Core.Entities;

public class UserQuiz : BaseEntity
{
    public long UserId { get; set; }
    public QuizType QuizType { get; set; }
    public long QuizId { get; set; }

    public DateTime Time { get; set; }

    public int Xp { get; set; }
    public int Type { get; set; }

    public int MarkPercentage { get; set; }
    public string Submissions { get; set; } // later: save submitted data as json string

    public UserQuiz(long userId)
    {
        Id = userId;
        UserId = userId;
        Time = DateTime.UtcNow;
    }
}