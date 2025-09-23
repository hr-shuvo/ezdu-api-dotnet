using System.ComponentModel.DataAnnotations;

namespace Core.DTOs;

public class TopicDto
{
    public long Id { get; set; }
    public string Name { get; set; }
    public string SubTitle { get; set; }
    
    public string Description { get; set; }
    
    [Required]
    public long? SubjectId { get; set; }
    
    [Required]
    public long? LessonId { get; set; }
    
    public int Status { get; set; }
}