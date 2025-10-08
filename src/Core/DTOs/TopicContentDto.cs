using System.ComponentModel.DataAnnotations;

namespace Core.DTOs;

public class TopicContentDto
{
    public long Id { get; set; }
    public string Name { get; set; }

    [Required]
    public int? Type { get; set; }

    public string Content { get; set; }

    public int Order { get; set; }
    
    [Required]
    public long? SubjectId { get; set; }
    [Required]
    public long? LessonId { get; set; }
    [Required]
    public long? TopicId { get; set; }

    public int Status { get; set; }
}