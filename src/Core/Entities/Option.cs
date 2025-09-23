using Core.App.DTOs.Common;

namespace Core.Entities;

public class Option : BaseEntity
{
    public bool IsCorrect { get; set; } = false;

    public long QuestionId { get; set; }
    public Question Question { get; set; }
}