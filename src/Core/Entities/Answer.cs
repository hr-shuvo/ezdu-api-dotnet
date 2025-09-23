using Core.App.DTOs.Common;

namespace Core.Entities;

public class Answer : BaseEntity
{
    public long QuestionId { get; set; }
    public Question Question { get; set; }
    
    public string ExpectedAnswer { get; set; }
}