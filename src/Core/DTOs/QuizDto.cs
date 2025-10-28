using System.ComponentModel.DataAnnotations;
using Core.Enums;

namespace Core.DTOs;

public class QuizDto
{
    public long Id { get; set; }
    [Required]
    public string Name { get; set; }
    
    public string Description { get; set; }
    public QuestionType Type { get; set; }
    public int TotalMarks { get; set; }
    public int PassingMarks { get; set; }
    
    [Required]
    public int DurationInMinutes { get; set; }
    public bool HasNegativeMarks { get; set; }
    
    [Required]
    public DateTime? StartTime { get; set; }
    public DateTime? EndTime { get; set; }
    
    public int Status { get; set; }

    public ICollection<long> QuestionIds { get; set; } = [];
}