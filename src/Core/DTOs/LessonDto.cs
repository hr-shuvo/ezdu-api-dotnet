using System.ComponentModel.DataAnnotations;
using Core.App.DTOs.Common;

namespace Core.DTOs;

public class LessonDto
{
    public long Id { get; set; }
    public string Name { get; set; }
    public string SubTitle { get; set; }

    public string Content { get; set; }

    [Required] public long SubjectId { get; set; }

    public int Status { get; set; }
}

public class LessonWithTopicsDto
{
    public long Id { get; set; }
    public string Name { get; set; }
    public List<TopicDtoMini> Topics { get; set; }
}

public class TopicDtoMini
{
    public long Id { get; set; }
    public string Name { get; set; }
}