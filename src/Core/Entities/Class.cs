using Core.App.DTOs.Common;
using Core.Enums;

namespace Core.Entities;

public class Class : BaseEntity
{
    public Segment Segment { get; set; }

    public string Groups { get; set; }

    public bool HasBatch { get; set; }

    public Class()
    {
        HasBatch = Segment is Segment.Ssc or Segment.Hsc;
    }
    
}