using Core.App.DTOs.Common;
using Core.Enums;

namespace Core.Entities;

public class Quiz : BaseEntity
{
    public string Description { get; set; }
    public QuizType Type { get; set; }
    public int TotalMarks { get; set; }
    public int PassingMarks { get; set; }
    public int DurationInMinutes { get; set; }
}