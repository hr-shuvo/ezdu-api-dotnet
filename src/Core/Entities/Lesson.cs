using Core.App.DTOs.Common;

namespace Core.Entities;

public class Lesson : BaseEntity
{
    public string Title { get; set; }
    public string SubTitle { get; set; }
    public string Content { get; set; }
    public string VideoUrl { get; set; }
    public string ResourceUrl { get; set; }
    
    public long SubjectId { get; set; }
}