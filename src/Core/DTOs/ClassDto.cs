using Core.App.Attributes;
using Core.App.DTOs.Common;

namespace Core.DTOs;

public class ClassDto : BaseEntity
{
    public int Segment { get; set; }

    [AllowedValueList("science", "commerce", "arts", "general")]
    public List<string> Groups { get; set; } = [];
    
    public bool HasBatch { get; set; }
    
}