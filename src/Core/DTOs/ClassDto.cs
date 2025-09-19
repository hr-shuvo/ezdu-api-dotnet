using Core.App.Attributes;

namespace Core.DTOs;

public class ClassDto
{
    public long Id { get; set; }
    public int Status { get; set; }
    
    public string Title { get; set; }
    public int Segment { get; set; }

    [AllowedValues("science", "commerce", "arts", "general")]
    public List<string> Groups { get; set; } = [];
    
    public bool HasBatch { get; set; }


    public DateTime? CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}