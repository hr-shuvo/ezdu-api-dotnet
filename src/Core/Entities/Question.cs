using Core.App.DTOs.Common;
using Core.Enums;

namespace Core.Entities;

public class Question : BaseEntity
{
    public long SubjectId { get; set; }
    public long LessonId { get; set; }
    public long TopicId { get; set; }
    
    public QuestionType QuestionType { get; set; }
    public string Passage { get; set; } = string.Empty;
    
    public string ImageUrl { get; set; }
    public string ImagePublicId { get; set; }

    public DifficultyLevel DifficultyLevel { get; set; } = DifficultyLevel.Medium;
    public int Marks { get; set; } = 1;
    public string Tags { get; set; } = string.Empty; // TODO: Change to List<string> later if needed

    // Hint & Explanation
    public string Hint { get; set; } = string.Empty;
    public string Explanation { get; set; } = string.Empty;
    public string ExplanationImageUrl { get; set; } = string.Empty;
    public string ExplanationImagePublicId { get; set; } = string.Empty;
    public string ExplanationVideoUrl { get; set; } = string.Empty;
    public string ExplanationResourceUrl { get; set; } = string.Empty;
    
    public ICollection<Option> Options { get; set; } = []; // MCQ / TrueFalse
    public ICollection<Answer> Answers { get; set; } = [];
    public ICollection<QuestionReference> References { get; set; } = [];
    
    /// <summary>
    /// Exam Archive, Question Bank
    /// </summary>
    public long ExamId { get; set; }
    
}