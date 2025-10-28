using Core.App.DTOs.Common;
using Core.Enums;

namespace Core.Entities;

public class Quiz : BaseEntity
{
    public string Description { get; set; }
    public QuestionType Type { get; set; }
    public int TotalMarks { get; set; }
    public int PassingMarks { get; set; }
    public int DurationInMinutes { get; set; }
    public bool HasNegativeMarks { get; set; }
    
    public DateTime? StartTime { get; set; }
    public DateTime? EndTime { get; set; }
    
    public ICollection<QuizQuestion> Questions { get; set; } = new List<QuizQuestion>();
}