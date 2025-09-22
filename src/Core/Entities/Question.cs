using Core.App.DTOs.Common;

namespace Core.Entities;

public class Question : BaseEntity
{
    public string Options { get; set; } // JSON string representing options
    public string CorrectAnswer { get; set; }
    public int Marks { get; set; }

    public long SubjectId { get; set; }
    public long LessonId { get; set; }
    public long TopicId { get; set; }

    public string Passage { get; set; }
    public string ImageUrl { get; set; }
    public string ImagePublicId { get; set; }
    
    public string Explanation { get; set; }
    public string ExplanationImageUrl { get; set; }
    public string ExplanationImagePublicId { get; set; }
    public string ExplanationVideoUrl { get; set; }
    public string ExplanationResourceUrl { get; set; }
    
    public string Hint { get; set; }
    public string DifficultyLevel { get; set; }
    public string QuestionType { get; set; }
    public string Tags { get; set; }
    
}