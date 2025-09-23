using System.ComponentModel.DataAnnotations;
using Core.App.Attributes;
using Core.App.DTOs.Common;

namespace Core.DTOs;

public class SubjectDto : BaseEntity
{
    public string SubTitle { get; set; }
    
    [Required]
    public string Code { get; set; }
    
    [AllowedValueList("science", "commerce", "arts", "general")]
    public List<string> Groups { get; set; } = [];
    
    public bool HasPaper { get; set; }
    public bool HasPractical { get; set; }
    
    [Required(ErrorMessage = "Class is required")]
    public long ClassId { get; set; }
    public int Segment { get; set; }
    

}