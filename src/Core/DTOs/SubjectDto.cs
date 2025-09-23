using System.ComponentModel.DataAnnotations;
using Core.App.DTOs.Common;
using Core.Enums;

namespace Core.DTOs;

public class SubjectDto : BaseEntity
{
    public string SubTitle { get; set; }
    public string Code { get; set; }
    
    [AllowedValues("science", "commerce", "arts", "general")]
    public List<string> Groups { get; set; } = [];
    
    public bool HasPaper { get; set; }
    public bool HasPractical { get; set; }
    
    public long ClassId { get; set; }
    public int Segment { get; set; }
    

}