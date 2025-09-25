using Core.App.DTOs.Common;
using Core.Enums;

namespace Core.Entities;

public class TopicContent : BaseEntity
{
    public ContentType Type { get; set; } = ContentType.ReadingText;

    public string Url { get; set; } // could be video link, document link, etc.
    public string Content { get; set; } // for textual content

    public int Order { get; set; }
    public long TopicId { get; set; }
    
}