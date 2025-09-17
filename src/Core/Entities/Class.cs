using Core.App.DTOs.Common;
using Core.Enums;

namespace Core.Entities;

public class Class : BaseEntity
{
    public string Title { get; set; }

    public Segment Segment { get; set; }

    public string Groups { get; set; }

    public bool HasBatch { get; set; }

    public Class()
    {
        HasBatch = Segment is Segment.SSC or Segment.HSC;
    }
    
}