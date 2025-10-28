using System.ComponentModel.DataAnnotations;
using Core.App.Attributes;
using Core.Enums;

namespace Core.DTOs;

[RequireAny(nameof(LessonId), nameof(ExamId), ErrorMessage = "Either Lesson or Exam must be selected.")]
public class QuestionDto
{
    public long Id { get; set; }
    public string Name { get; set; }

    [Range(0, 5)] public QuestionType QuestionType { get; set; }
    public string Passage { get; set; }

    [Required] public long? SubjectId { get; set; }
    public long? LessonId { get; set; }
    public long? TopicId { get; set; }

    /// <summary>
    /// Exam Archive
    /// </summary>
    public long? ExamId { get; set; }

    [Range(0, 5)] public int DifficultyLevel { get; set; }

    [Range(1, 20)] public int Marks { get; set; }

    // public string Tags { get; set; }

    // Hint & Explanation
    public string Hint { get; set; }
    public string Explanation { get; set; }


    public ICollection<OptionDto> Options { get; set; } = [];
}