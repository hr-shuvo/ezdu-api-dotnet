using Core.App.DTOs.Common;

namespace Core.Entities;

public class Topic : BaseEntity
{
    public string Title { get; set; }
    public string Description { get; set; }
    
    public long SubjectId { get; set; }
    public long LessonId { get; set; }
    
    public int Order { get; set; }
    
    
}