namespace Core.DTOs;

public class OptionDto
{
    public long Id { get; set; }
    public string Name { get; set; }
    public bool IsCorrect { get; set; }

    // [Required] // TODO - make separate DTO for update and create question
    public long? QuestionId { get; set; }
}