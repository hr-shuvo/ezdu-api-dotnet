using Core.Entities;
using Core.Enums;

namespace Core.DTOs;

public class QuizResponseDto
{
    public long Id { get; set; }
    public string Name { get; set; }
    
    public string Description { get; set; }
    public QuestionType Type { get; set; }
    public int TotalMarks { get; set; }
    public int PassingMarks { get; set; }
    public int DurationInMinutes { get; set; }
    public bool HasNegativeMarks { get; set; }
    public DateTime? StartTime { get; set; }
    public DateTime? EndTime { get; set; }
    
    public int Status { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public long? CreatedBy { get; set; }
    public long? UpdatedBy { get; set; }

    public ICollection<Question> Questions { get; set; }
}