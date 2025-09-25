using System.ComponentModel.DataAnnotations;

namespace Core.DTOs;

public class OptionDto
{
    public long Id { get; set; }
    public string Text { get; set; }
    public bool IsCorrect { get; set; }

    [Required]
    public long? QuestionId { get; set; }
}