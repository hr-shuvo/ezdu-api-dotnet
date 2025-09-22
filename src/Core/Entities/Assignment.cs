using Core.App.DTOs.Common;
using Core.Enums;

namespace Core.Entities;

public class Assignment : BaseEntity
{
    public string Description { get; set; }

    public AssignmentType Type { get; set; }

    public long SubjectId { get; set; }
    public long LessonId { get; set; }
    
}